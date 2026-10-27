# API de Relógios

**Tema:** relógios
**Recurso principal:** `relogios`

## Objetivo

Minimal API em ASP.NET Core que gerencia um catálogo de relógios com CRUD completo
(listar, buscar por id, cadastrar, atualizar e remover), baseada no projeto
[API Biblioteca V2](https://github.com/lucasfogacadj/ApiBibliotecaV2) visto em aula.

- **AP1:** CRUD com dados em memória (`List<RelogioDto>`).
- **AP2:** a lista em memória foi substituída por persistência em **SQLite** com
  **Entity Framework Core**, e foi adicionado um **frontend** (HTML/CSS/JS) servido pela
  mesma origem da API e ligado aos endpoints reais.

Cada relógio possui: `id`, `marca`, `modelo`, `tipo` (ex.: Digital, Automático, Quartzo, Smartwatch) e `preco`.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Ferramenta do EF Core (para aplicar as migrations):
  ```bash
  dotnet tool install --global dotnet-ef
  ```
- [Bruno](https://www.usebruno.com/downloads) (para os testes da API)
- Um navegador

## Como executar

```bash
# 1. restaurar pacotes e compilar
dotnet build

# 2. criar o banco SQLite (relogios.db) a partir das migrations versionadas
dotnet ef database update

# 3. iniciar a API + frontend
dotnet run --urls http://localhost:5050
```

O passo 2 cria o arquivo `relogios.db` com a tabela `Relogios` e 3 relógios iniciais
(Casio, Seiko e Apple). O arquivo do banco **não** é versionado (está no `.gitignore`).
Para começar do zero, apague `relogios.db` e rode `dotnet ef database update` de novo.

**URL local:** http://localhost:5050

## Como abrir a interface

Com a API rodando, abra no navegador: **http://localhost:5050**

A interface (pasta `wwwroot/`) é servida pela própria API (`UseDefaultFiles` + `UseStaticFiles`),
então frontend e backend ficam na mesma origem e não é preciso configurar CORS. Pela tela é possível:

- **listar** os relógios gravados no banco (`GET /api/relogios`);
- **cadastrar** pelo formulário (`POST /api/relogios`);
- **editar** clicando em *Editar*, alterando os campos e salvando (`GET /api/relogios/{id}` + `PUT /api/relogios/{id}`);
- **excluir** clicando em *Excluir* (`DELETE /api/relogios/{id}`).

Após cada operação a tela mostra o código HTTP retornado e recarrega a lista a partir da API.

## Endpoints

| Método | Rota | Descrição | Respostas |
|--------|------|-----------|-----------|
| GET | `/` | Interface web (frontend) | 200 OK |
| GET | `/api` | Informa que a API está no ar | 200 OK |
| GET | `/api/relogios` | Lista todos os relógios | 200 OK |
| GET | `/api/relogios/{id}` | Busca um relógio pelo id | 200 OK / 404 Not Found |
| POST | `/api/relogios` | Cadastra um novo relógio | 201 Created |
| PUT | `/api/relogios/{id}` | Atualiza um relógio existente | 200 OK / 404 Not Found |
| DELETE | `/api/relogios/{id}` | Remove um relógio | 204 No Content / 404 Not Found |

> Mudança desde a AP1: a mensagem "API no ar" saiu de `/` e foi para `/api`, porque `/` agora
> entrega a interface. As rotas `/api/relogios` continuam iguais.

## Exemplos de JSON

**POST** `/api/relogios`

```json
{
  "marca": "Orient",
  "modelo": "Bambino",
  "tipo": "Automático",
  "preco": 1290.00
}
```

**PUT** `/api/relogios/{id}`

```json
{
  "marca": "Orient",
  "modelo": "Bambino Version 2",
  "tipo": "Automático",
  "preco": 1190.00
}
```

## Estrutura do projeto

```
ApiRelogios/
├── Program.cs              # configuração do EF Core, arquivos estáticos e rotas da API
├── Models/Relogio.cs       # Model (entidade) gravada no banco
├── Data/AppDbContext.cs    # DbContext com DbSet<Relogio> e dados iniciais (HasData)
├── Migrations/             # migrations do EF Core (criam a tabela Relogios)
├── wwwroot/                # frontend: index.html, style.css, app.js
├── bruno/                  # Collection do Bruno
├── appsettings.json        # connection string: Data Source=relogios.db
└── ApiRelogios.csproj      # pacotes Microsoft.EntityFrameworkCore.Sqlite e .Design
```

- `Relogio` — Model persistida pelo EF Core (`Id` é a chave primária gerada pelo banco).
- `AppDbContext` — DbContext; registrado em `Program.cs` com `UseSqlite`.
- `RelogioDto` — DTO de saída (com `Id`).
- `RelogioEntradaDto` — DTO de entrada usado no POST e no PUT (sem `Id`).

Criar, atualizar e excluir chamam `SaveChangesAsync()`, então os dados ficam gravados em
`relogios.db` e **continuam disponíveis depois de reiniciar a API**.

## Collection de testes (Bruno)

A Collection do Bruno está na pasta [`bruno/`](bruno/) deste repositório. Para usar:
abra o Bruno → **Open Collection** → selecione a pasta `bruno` → escolha o environment **Local**
(`baseUrl = http://localhost:5050`).

| Requisição | Resultado esperado |
|------------|--------------------|
| 00 - API no ar | 200 OK |
| 01 - Listar | 200 OK |
| 02 - Buscar por id | 200 OK |
| 03 - Cadastrar | 201 Created |
| 04 - Atualizar | 200 OK |
| 05 - Remover | 204 No Content |
| 06 - Confirmar remoção | 404 Not Found |

Execute na ordem. O **03 - Cadastrar** guarda o `id` gerado pelo banco na variável
`relogioId`, que é usada pelo PUT, DELETE e pela confirmação do 404 — por isso a collection
funciona mesmo com outros registros já gravados no banco. Cada requisição tem um teste do
código de status esperado.

## Vídeo de demonstração — AP2

🎥 Link do vídeo: **(inserir link público do vídeo da AP2)**

## Vídeo de demonstração — AP1

🎥 Link do vídeo: https://drive.google.com/drive/folders/1fI53KGtHq-L2KF24BdKPERNm8qpwRPJn?usp=drive_link
