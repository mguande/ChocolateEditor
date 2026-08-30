# Gera um executavel unico (nao precisa do .NET instalado) e, se o Inno Setup
# estiver presente, um instalador Setup.exe tambem unico.
$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
$outDir = Join-Path $PSScriptRoot "out"
$distDir = Join-Path $PSScriptRoot "dist"
$exeName = "ChocolatePlusEditor.exe"

Remove-Item $outDir, $distDir -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $outDir | Out-Null
New-Item -ItemType Directory -Path $distDir | Out-Null

Write-Host "A publicar Chocolate Plus Editor (win-x64, ficheiro unico)..."
dotnet publish (Join-Path $root "ChocolateEditor.csproj") `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -p:DebugType=none `
    -p:DebugSymbols=false `
    -o $outDir

$built = Join-Path $outDir "ChocolateEditor.exe"
if (-not (Test-Path $built)) {
    throw "Publish falhou: $built nao encontrado."
}

# Nunca empacotar configs locais (chaves, presets, dumps)
Get-ChildItem $outDir -Recurse -Force -ErrorAction SilentlyContinue |
    Where-Object {
        $_.Name -in @(
            "ai-settings.json",
            "matribox-presets.json",
            "matribox-presets.bak.json",
            "last-dump.bin",
            "last-matribox-sysex.bin"
        ) -or $_.Name -like "*.modelo" -or $_.Name -eq "config"
    } |
    Remove-Item -Recurse -Force

$portable = Join-Path $distDir $exeName
Copy-Item $built $portable -Force
Write-Host "Portatil: $portable"

$iscc = @(
    "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
    "$env:ProgramFiles\Inno Setup 6\ISCC.exe"
) | Where-Object { Test-Path $_ } | Select-Object -First 1

if ($iscc) {
    Write-Host "A gerar instalador Inno Setup..."
    & $iscc (Join-Path $PSScriptRoot "ChocolateEditor.iss")
    Write-Host "Instalador em $distDir"
} else {
    Write-Host "Inno Setup nao encontrado. O download e o executavel unico $exeName (nao precisa instalar)."
}

Get-ChildItem $distDir | Format-Table Name, Length, LastWriteTime
