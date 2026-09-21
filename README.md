# API de Relógios

**Tema:** relógios
**Recurso principal:** `relogios`

## Objetivo

Minimal API em ASP.NET Core que gerencia um catálogo de relógios com CRUD completo
(listar, buscar por id, cadastrar, atualizar e remover). Trabalho AP1, baseado no projeto
[API Biblioteca V2](https://github.com/lucasfogacadj/ApiBibliotecaV2) visto em aula.

Cada relógio possui: `id`, `marca`, `modelo`, `tipo` (ex.: Digital, Automático, Quartzo, Smartwatch) e `preco`.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Bruno](https://www.usebruno.com/downloads) (para os testes)

## Como executar

```bash
dotnet --version
dotnet build
dotnet run --urls http://localhost:5050
```

**URL local usada nos testes:** http://localhost:5050

## Endpoints

| Método | Rota | Descrição | Respostas |
|--------|------|-----------|-----------|
| GET | `/` | Informa que a API está no ar | 200 OK |
| GET | `/api/relogios` | Lista todos os relógios | 200 OK |
| GET | `/api/relogios/{id}` | Busca um relógio pelo id | 200 OK / 404 Not Found |
| POST | `/api/relogios` | Cadastra um novo relógio | 201 Created |
| PUT | `/api/relogios/{id}` | Atualiza um relógio existente | 200 OK / 404 Not Found |
| DELETE | `/api/relogios/{id}` | Remove um relógio | 204 No Content / 404 Not Found |

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

**PUT** `/api/relogios/4`

```json
{
  "marca": "Orient",
  "modelo": "Bambino Version 2",
  "tipo": "Automático",
  "preco": 1190.00
}
```

## Estrutura do código

- `record RelogioDto` — DTO completo (com `Id`).
- `record RelogioEntradaDto` — DTO de entrada usado no POST e no PUT (sem `Id`).
- `List<RelogioDto>` — armazena os relógios em memória, com 3 registros iniciais.

> ⚠️ **Os dados ficam somente em memória.** Tudo o que for cadastrado, alterado ou removido
> é perdido quando a aplicação é reiniciada.

## Collection de testes (Bruno)

A Collection do Bruno está na pasta [`bruno/`](bruno/) deste repositório. Para usar:
abra o Bruno → **Open Collection** → selecione a pasta `bruno` → escolha o environment **Local**
(`baseUrl = http://localhost:5050`).

| Requisição | Resultado esperado |
|------------|--------------------|
| 00 - API no ar | 200 OK |
| 01 - Listar | 200 OK |
| 02 - Buscar por id | 200 OK |
| 03 - Cadastrar | 201 Created (id 4) |
| 04 - Atualizar | 200 OK |
| 05 - Remover | 204 No Content |
| 06 - Confirmar remoção | 404 Not Found |

Execute na ordem, logo após iniciar a API (o relógio criado recebe o id 4).

## Vídeo de demonstração

🎥 Link do vídeo: **COLE_AQUI_O_LINK_PUBLICO_DO_VIDEO**
