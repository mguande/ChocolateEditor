# Gera o executavel e o instalador, faz commit, tag e release no GitHub.
#
# Na raiz do repositorio:
#   .\deploy\release.ps1
#   .\deploy\release.ps1 -Bump minor
#   .\deploy\release.ps1 -Version 1.4.0 -Notes "O que mudou nesta versao."
#
# Sem -Version, aumenta o ultimo numero (1.3.0 vira 1.3.1).
# Precisa do Inno Setup 6 e de credencial do GitHub ja usada pelo git.
param(
    [string]$Version,
    [ValidateSet("patch", "minor", "major")]
    [string]$Bump = "patch",
    [string]$Notes,
    [string]$NotesFile
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

function Read-Text([string]$Path) {
    return [System.IO.File]::ReadAllText($Path)
}

function Write-Text([string]$Path, [string]$Text) {
    $utf8 = New-Object System.Text.UTF8Encoding $false
    [System.IO.File]::WriteAllText($Path, $Text, $utf8)
}

function Get-CurrentVersion {
    $csproj = Read-Text (Join-Path $root "ChocolateEditor.csproj")
    $match = [regex]::Match($csproj, "<Version>([^<]+)</Version>")
    if (-not $match.Success) {
        throw "Nao encontrei <Version> no ChocolateEditor.csproj."
    }
    return $match.Groups[1].Value.Trim()
}

function Get-NextVersion([string]$Current, [string]$Part) {
    $bits = $Current.Split(".")
    if ($bits.Count -lt 3) {
        throw "Versao atual invalida: $Current"
    }
    $major = [int]$bits[0]
    $minor = [int]$bits[1]
    $patch = [int]$bits[2]
    switch ($Part) {
        "major" { return "$($major + 1).0.0" }
        "minor" { return "$major.$($minor + 1).0" }
        default { return "$major.$minor.$($patch + 1)" }
    }
}

function Update-VersionFiles([string]$NewVersion) {
    $csprojPath = Join-Path $root "ChocolateEditor.csproj"
    $csproj = Read-Text $csprojPath
    $csproj = [regex]::Replace($csproj, "(<Version>)[^<]+(</Version>)", "`${1}$NewVersion`${2}")
    $csproj = [regex]::Replace($csproj, "(<AssemblyVersion>)[^<]+(</AssemblyVersion>)", "`${1}$NewVersion.0`${2}")
    $csproj = [regex]::Replace($csproj, "(<FileVersion>)[^<]+(</FileVersion>)", "`${1}$NewVersion.0`${2}")
    Write-Text $csprojPath $csproj

    $issPath = Join-Path $root "deploy\ChocolateEditor.iss"
    $iss = Read-Text $issPath
    $iss = [regex]::Replace($iss, '(#define MyAppVersion ")[^"]+(")', "`${1}$NewVersion`${2}")
    Write-Text $issPath $iss

    $readmePath = Join-Path $root "README.md"
    $readme = Read-Text $readmePath
    $readme = [regex]::Replace($readme, "\*\*Versão \d+\.\d+\.\d+\*\*", "**Versão $NewVersion**")
    Write-Text $readmePath $readme

    $catalogPath = Join-Path $root "Music\MusicCatalog.cs"
    $catalog = Read-Text $catalogPath
    $catalog = [regex]::Replace($catalog, "ChocolatePlusEditor/\d+\.\d+\.\d+", "ChocolatePlusEditor/$NewVersion")
    Write-Text $catalogPath $catalog
}

function Get-ReleaseNotes([string]$NewVersion) {
    if ($NotesFile) {
        if (-not (Test-Path $NotesFile)) {
            throw "Ficheiro de notas nao encontrado: $NotesFile"
        }
        return (Read-Text $NotesFile).Trim()
    }

    $body = "Chocolate Plus Editor $NewVersion`r`n`r`n"
    if ($Notes) {
        $body += $Notes.Trim() + "`r`n`r`n"
    }
    $body += @"
- ChocolatePlusEditor.exe — um unico ficheiro (Windows 10/11 64-bit). Nao precisa de instalar o .NET
- ChocolatePlusEditor-Setup.exe — instalador

O Windows pode avisar que a app nao e reconhecida. Escolhe Mais informacoes e Executar na mesma.
"@
    return $body.Trim()
}

function Publish-GitHubRelease([string]$Tag, [string]$Title, [string]$Body, [string[]]$Assets) {
    $gh = Get-Command gh -ErrorAction SilentlyContinue
    if ($gh) {
        $notesPath = Join-Path $env:TEMP "chocolate-release-notes.txt"
        Write-Text $notesPath $Body
        & gh release create $Tag $Assets --repo "mguande/ChocolateEditor" --title $Title --notes-file $notesPath
        if ($LASTEXITCODE -ne 0) {
            throw "gh release create falhou."
        }
        return
    }

    $credText = "protocol=https`nhost=github.com`n`n" | git credential fill
    $passwordLine = $credText | Where-Object { $_ -like "password=*" } | Select-Object -First 1
    if (-not $passwordLine) {
        throw "Sem credencial do GitHub. Faz git push uma vez ou instala o GitHub CLI (gh)."
    }
    $token = $passwordLine.Substring(9)
    $headers = @{
        Authorization          = "Bearer $token"
        Accept                 = "application/vnd.github+json"
        "X-GitHub-Api-Version" = "2022-11-28"
    }
    $payload = @{
        tag_name         = $Tag
        target_commitish = "main"
        name             = $Title
        body             = $Body
        draft            = $false
        prerelease       = $false
    } | ConvertTo-Json
    $release = Invoke-RestMethod -Method Post -Uri "https://api.github.com/repos/mguande/ChocolateEditor/releases" -Headers $headers -Body $payload -ContentType "application/json; charset=utf-8"
    $upload = $release.upload_url.Replace("{?name,label}", "")
    foreach ($asset in $Assets) {
        $name = Split-Path $asset -Leaf
        Write-Host "A enviar $name..."
        $code = curl.exe -sS -o NUL -w "%{http_code}" -X POST `
            -H "Authorization: Bearer $token" `
            -H "Accept: application/vnd.github+json" `
            -H "Content-Type: application/octet-stream" `
            --data-binary "@$asset" `
            "$upload`?name=$name"
        if ($code -ne "201") {
            throw "Upload de $name falhou (HTTP $code)."
        }
    }
    Write-Host $release.html_url
}

$branch = (git rev-parse --abbrev-ref HEAD).Trim()
if ($branch -ne "main") {
    throw "Este script publica a partir de main. Estas em '$branch'."
}

$current = Get-CurrentVersion
if (-not $Version) {
    $Version = Get-NextVersion $current $Bump
}
if ($Version -notmatch "^\d+\.\d+\.\d+$") {
    throw "Versao invalida: $Version. Usa o formato 1.2.3."
}

$tag = "v$Version"
$existing = git tag -l $tag
if ($existing) {
    throw "A tag $tag ja existe."
}

Write-Host "Versao $current -> $Version"
Update-VersionFiles $Version

Write-Host "A gerar executavel e instalador..."
& (Join-Path $PSScriptRoot "publish.ps1")
if ($LASTEXITCODE -ne 0) {
    throw "publish.ps1 falhou."
}

$portable = Join-Path $root "deploy\dist\ChocolatePlusEditor.exe"
$setup = Join-Path $root "deploy\dist\ChocolatePlusEditor-Setup.exe"
if (-not (Test-Path $portable) -or -not (Test-Path $setup)) {
    throw "Faltou o executavel ou o instalador em deploy\dist. Confirma que o Inno Setup 6 esta instalado."
}

git add -A
$pending = git status --porcelain
if (-not $pending) {
    throw "Nada para commit. A versao ja estava em $Version e nao ha outras alteracoes."
}

git commit -m "Publicar a versao $Version."
git tag $tag
git push origin HEAD
git push origin $tag

$notes = Get-ReleaseNotes $Version
Publish-GitHubRelease $tag "Chocolate Plus Editor $Version" $notes @($portable, $setup)
Write-Host "Release $tag publicada."
Write-Host "https://github.com/mguande/ChocolateEditor/releases/tag/$tag"
