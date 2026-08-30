# Chocolate Plus Editor

Editor Windows (gratuito) para o pedal **M-VAVE Chocolate Plus** (`FootCtrlPlus`) e para cenas da **Sonicake Matribox II Pro**. Serve para ler, editar, copiar bancos e gravar a configuração no Chocolate por **USB MIDI**, mapear switches para patches/efeitos da Matribox, e gerar patches com IA.

**Versão 1.2.0** · **Windows 10/11 64-bit** · não precisa de instalar o .NET · um único ficheiro para descarregar

## Download

**[Descarregar ChocolatePlusEditor.exe](https://github.com/mguande/ChocolateEditor/releases/latest/download/ChocolatePlusEditor.exe)** — um só ficheiro (~50 MB), Windows 10/11 64-bit. Não precisa de instalar o .NET: descarrega e abre.

Todas as versões: [Releases](https://github.com/mguande/ChocolateEditor/releases/latest)

O Windows pode avisar que a app não é reconhecida (não está assinada). Escolhe **Mais informações** → **Executar na mesma**.

---

## O que faz

- Liga ao Chocolate Plus por cabo USB-C (porta MIDI `FootCtrlPlus`)
- Lê e grava bancos / grupos (E/F no pedal)
- Edita os quatro switches (A–D): A Bank (pressionar / curto) e B Bank (soltar / longo)
- Mensagens **PC**, **CC**, **Note On/Off** e **SysEx**
- Copia um banco para outro (ou para um intervalo)
- Guarda e abre ficheiros `.choco.json` no PC
- Modos do CubeSuite (1–12), incluindo **Advanced Custom 1 e 2**
- **Chocolate × Matribox**: mapeia bancos/patches (PC 0-based) e liga/desliga os 12 módulos (CC 43–54). O ecrã só desbloqueia depois de ligar o Chocolate
- **Configuração Matribox**: importa `.prst` da Sonicake (nomes, cadeia, On/Off)
- **Prompt IA**: ChatGPT, Claude ou Gemini geram um patch Matribox (cadeia visual + knobs) e podes guardar `.prst`

Não substitui o firmware. É um editor da memória USB, no espírito do [CubeSuite](http://www.m-vave.com/appdownload) oficial. MIDI da Matribox conforme o [manual firmware 1.2.2](https://sonicake.oss-accelerate.aliyuncs.com/res/JIyz39IVZe/Matribox_II_PRO_Online_Manual_EN_Firmware_V1.2.2.pdf).

---

## Manual de utilização

### 1. Ligar o pedal

1. No Chocolate Plus, põe o interruptor em **U** (USB).
2. Usa um cabo USB-C **de dados** (não só de carga).
3. No Windows, o dispositivo deve aparecer como **FootCtrlPlus**.
4. Abre o Chocolate Plus Editor.
5. Em **MIDI USB**, escolhe `FootCtrlPlus` (se não aparecer, clica **Atualizar**).
6. Clica **Conectar**. O estado deve passar a *Chocolate detectado*.

Se não ligar: troca de cabo, porta USB, ou fecha o CubeSuite (só uma app de cada vez no MIDI).

### 2. Cenas Matribox II (Chocolate a controlar a pedaleira)

1. No ecrã inicial escolhe **Chocolate × Matribox**.
2. Liga o **Chocolate** (switch U) e clica **Conectar** — a UI fica bloqueada até isso.
3. Liga a **Matribox** por USB (Global → MIDI → MIDI In Source = USB ou Mixed). Fecha o editor oficial da Sonicake.
4. **Ler Chocolate** se já tiveres bancos; senão parte do template. O combo de bancos começa em **Selecione**.
5. Na lista, escolhe o banco/patch da Matribox (01-A … 60-D). Os 240 presets usam CC0 0/1 + PC 0–119.
6. **Switch A (toque curto)** — edita os 12 efeitos (CC 43–54). Se o patch já estiver mapeado, o editor avisa antes de substituir.
7. **Switch A (toque longo)** — CC 0 + PC (escolhe o preset na Matribox).
8. Ou **A–D = patches A–D deste banco** para mapear os quatro switches de uma vez.
9. **Testar na Matribox** envia o MIDI já; **Gravar no Chocolate** grava no pedal.

Não dispares os 12 CC de uma vez em excesso: a Matribox pode sobrecarregar. O editor envia com pausa entre mensagens.

### 3. Configuração Matribox (importar `.prst`)

1. No ecrã inicial escolhe **Configuração Matribox**.
2. Importa a pasta de patches do editor Sonicake (ex.: `C:\Programas\Sonicake\Matribox 2 Pro\Patchs\meus`).
3. Os nomes e a cadeia (incluindo On/Off) ficam no banco local `matribox-presets.json` para o ecrã Chocolate × Matribox os mostrar.

### 4. Prompt IA (gerar patch Matribox)

1. No ecrã inicial escolhe **Prompt IA**.
2. **Configurar** — cola as chaves de API (ChatGPT, Claude e/ou Gemini). Ficam só neste PC.
3. Escreve o que queres (ex.: *som Nirvana, overdrive + delay curto*).
4. Deixa **Gerar patch Matribox II Pro** marcado para a IA devolver a cadeia e os knobs.
5. **Enviar manual e knobs** — desligado por omissão (prompt leve). Liga só se quiseres o manual completo e todos os knobs por algoritmo (mais pesado, melhor contexto).
6. **Enviar** (ou Ctrl+Enter). A cadeia aparece com pedais/amp/cab e knobs.
7. **Guardar .prst** — grava o patch e um `.txt` com a pergunta e a resposta. Copia o `.prst` para a pasta Patchs do editor Sonicake.

Modelos por omissão: `gpt-4o-mini`, `claude-sonnet-4-5`, `gemini-2.5-flash`.

### 5. Ler o que está no pedal

1. No ecrã inicial escolhe **Chocolate Plus** (editor USB).
2. Clica **Ler do Chocolate**.
3. Espera pela leitura (pode demorar; o log em baixo mostra o progresso).
4. O **Grupo 1** deve mostrar as listas do **Switch A** (e B/C/D se existirem no dump).
5. Clica nos grupos à esquerda para ver os outros bancos.

Confirma o **Modo** no topo (por exemplo *4. Advanced Custom 1* ou *5. Advanced Custom 2*). O modo 5 é o que usa **E/F** para mudar de banco no pedal.

### 6. Editar um banco

1. Escolhe o grupo na lista da esquerda.
2. Dá um **nome** ao banco (só no editor / JSON; o pedal não mostra este nome).
3. Em cada switch (A–D):
   - **Modo** do switch: toque simples, alternar 2 grupos, pressionar/soltar, longo, curto+longo.
   - **A Bank**: mensagens ao pressionar / toque curto.
   - **B Bank**: mensagens ao soltar / toque longo.
4. **Adicionar / Editar / Remover** nas listas. Duplo clique também edita.
5. Tipos de mensagem:
   - **Program Change (PC)** — número de preset 0–127
   - **Control Change (CC)** — número e valor (ex.: CC 43 = 127)
   - **Note On / Note Off**
   - **SysEx** — bytes em hexadecimal

### 7. Copiar um banco

1. Menu **Bancos** → **Copiar este banco…** (ou o botão no painel).
2. Escolhe origem e destino.
3. Opcional: copiar um intervalo de bancos e quais switches (A–D) / A Bank / B Bank.

Útil para repetir o mesmo patch em vários grupos e depois só mudar o PC/CC.

### 8. Gravar no pedal

1. Clica **Gravar no Chocolate**.
2. Não desligues o USB até o log indicar que gravou.
3. Testa no pedal. Se algo correr mal, volta a **Ler** (ou usa um `.choco.json` que tenhas gravado antes).

**Gravar substitui a memória do pedal.** Guarda primeiro um JSON no PC (**Arquivo → Salvar**).

### 9. Guardar no computador

- **Arquivo → Salvar / Salvar como** — ficheiro `.choco.json` (podes versionar no Git ou enviar a um amigo).
- **Arquivo → Abrir** — carrega um JSON e, se quiseres, grava no pedal.
- **Arquivo → Novo** — template inicial (não lê o pedal).

### 10. TRS e quantidade de bancos

- **TRS MIDI** vs expressão — jack traseiro.
- **Qtd. de bancos** — quantos grupos a lista mostra (no dump USB o Chocolate Plus usa até 8 neste editor).

---

## Requisitos

- Windows 10 ou 11 **64-bit**
- Chocolate Plus em modo USB (**U**)
- Cabo USB-C com dados

Não é preciso instalar o [.NET 9](https://dotnet.microsoft.com/download) se usares o `.exe` das [Releases](https://github.com/mguande/ChocolateEditor/releases/latest) (já vai tudo dentro do ficheiro).

Para **compilar** a partir do código: [SDK .NET 9](https://dotnet.microsoft.com/download) e Visual Studio 2022 (Designer WinForms) ou `dotnet build`.

---

## Compilar e gerar o instalador

```powershell
git clone https://github.com/mguande/ChocolateEditor.git
cd ChocolateEditor
.\deploy\publish.ps1
```

Resultado em `deploy\dist\`:

- `ChocolatePlusEditor.exe` — um único ficheiro
- `ChocolatePlusEditor-Setup.exe` — se o [Inno Setup 6](https://jrsoftware.org/isinfo.php) estiver instalado

### Configuração local (não vai para o Git)

Na pasta `config/` do repositório há **exemplos** (`*.modelo`) para clonar e preencher. Os ficheiros reais ficam com os **mesmos nomes de sempre**, só neste PC:

| Exemplo no Git | Ficheiro local (gitignored) |
| --- | --- |
| `config/ai-settings.json.modelo` | `%LocalAppData%\ChocolateEditor\ai-settings.json` — chaves de API |
| `config/matribox-presets.json.modelo` | `%LocalAppData%\ChocolateEditor\matribox-presets.json` — banco local dos 240 patches |

Dumps USB (`last-dump.bin`, `last-matribox-sysex.bin`) ficam na mesma pasta LocalAppData. Copia o `.modelo` para esse caminho se quiseres começar a partir do exemplo; a app também cria o JSON ao gravar as chaves.

---

## Notas e limitações

- O protocolo USB é o SysEx M-VAVE (família CUVAVE), o mesmo tipo de empacotamento 7-bit usado noutros produtos da marca.
- O layout da EEPROM foi inferido a partir do CubeSuite e de dumps reais. Switches B/C/D e grupos 2–8 dependem do que o pedal devolver na leitura.
- Não envies dumps nem `ai-settings.json` com dados que não queiras públicos.
- Projeto independente, **não** afiliado à M-VAVE nem à Sonicake. O software oficial continua a ser o CubeSuite / editor Matribox.

Manual do pedal (modos 1–12): [Chocolate Plus — instruções de software](https://manualf.oss-cn-hongkong.aliyuncs.com/manual/CUBE-TURNER/Chocolate%20Plus-Software%20instructions.pdf)

---

## Licença

Código neste repositório: usa e modifica à vontade. O nome e o hardware M-VAVE / Chocolate Plus / Sonicake Matribox pertencem aos respectivos donos.
