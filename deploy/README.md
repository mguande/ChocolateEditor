# Pasta deploy

Gera o executável único e, se existir Inno Setup 6, o instalador.

```powershell
.\deploy\publish.ps1
```

| Saída | Descrição |
| --- | --- |
| `dist/ChocolatePlusEditor.exe` | App portátil (um ficheiro) |
| `dist/ChocolatePlusEditor-Setup.exe` | Instalador (só com [Inno Setup 6](https://jrsoftware.org/isinfo.php)) |
| `out/` | Intermédio do `dotnet publish` (não vai para o Git) |
