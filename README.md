# MultipleFileReader

Mini sistema que demonstra a diferença entre **processamento sequencial** e **processamento paralelo** (CPU-bound) de arquivos enviados por upload.

Para cada arquivo recebido, o sistema calcula o **hash SHA-256** do conteúdo — uma operação que consome CPU — e devolve as informações do arquivo (nome, tamanho, tipo, hash e data). O objetivo é comparar o tempo gasto quando os arquivos são processados um a um versus em paralelo.

## Funcionalidades

- Upload de **múltiplos arquivos** (input `multiple`, com drag & drop no front).
- Dois caminhos de processamento:
  - **Sequencial**: percorre os arquivos com um `foreach`.
  - **Paralelo (CPU-bound)**: usa `Parallel.ForEach` + `ConcurrentBag`.
- Cálculo do hash SHA-256 de cada arquivo.
- Cronometragem (`Stopwatch`) impressa no console do servidor para comparar os tempos.
- Front simples servido pelo próprio backend (página estática).

## Estrutura

```
MultipleFileReader/
├── Controllers/MultipleFileReader.cs   # endpoints de upload
├── Services/FileService.cs            # lógica de processamento (seq. e paralelo)
├── Interface/IFileServices.cs         # contrato do serviço
├── FileModel.cs                       # modelo retornado ao cliente
├── FileUploadOperationFilter.cs       # ajuste do Swagger p/ upload multiple
├── wwwroot/index.html                 # frontend (input multiple + botões)
└── Program.cs                         # configuração da app (swagger, estáticos)
```

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`  | `/` | Frontend (página estática) |
| `GET`  | `/swagger` | Swagger UI |
| `POST` | `/MultipleFileReader/upload` | Processamento **sequencial** |
| `POST` | `/MultipleFileReader/upload-parallel` | Processamento **paralelo** (CPU-bound) |

Ambos recebem `multipart/form-data` com o campo `files` (um ou mais arquivos) e retornam JSON:

```json
[
  {
    "name": "exemplo.txt",
    "size": "12.00 KB",
    "type": "text/plain",
    "modified": "2026-09-11 10:30:00",
    "hash": "A1B2C3..."
  }
]
```

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)

## Como rodar

Na pasta do projeto:

```powershell
dotnet run
```

Acesse `http://localhost:5264/` (ou `https://localhost:7212/` se usar o profile `https`).

Para rodar com HTTPS:

```powershell
dotnet run --launch-profile https
```

> Abra o endereço **no navegador** (não dê duplo clique no `wwwroot/index.html`, pois abrir por `file://` gera erro de CORS/origem).

## Como testar e observar os resultados

1. Na página, arraste ou selecione **vários arquivos** (idealmente arquivos maiores ou muitos arquivos, para o hash demorar o suficiente e a diferença aparecer).
2. Clique em **Enviar** → executa o caminho **sequencial**.
3. Observe no **console do servidor** (terminal do `dotnet run`) a linha:

   ```
   Processamento sequencial (CPU-bound): X ms
   ```

4. Clique em **Enviar em paralelo** → executa o caminho **paralelo** com `Parallel.ForEach`.
5. Observe no console:

   ```
   Processamento paralelo (CPU-bound): Y ms
   ```

6. A tabela na página será preenchida com os dados processados (incluindo o hash SHA-256) após o envio bem-sucedido.

### O que observar

- A **tabela** mostra os dados retornados pelo servidor (prova de que os arquivos foram processados).
- O **console** mostra o tempo de cada abordagem.
- Para ver o paralelo ser mais rápido que o sequencial, use um volume de arquivos ou tamanhos que tornem o cálculo de hash relevante (com poucos arquivos pequenos, o custo de criar/gerenciar as threads paralelas pode, na prática, igualar ou até superar o sequencial — comportamento esperado em trabalho CPU-bound trivial).

## Observações técnicas

- Como o trabalho dominante é **CPU-bound** (hash), não se usa `async`/`await` no caminho paralelo: o paralelismo real vem do `Parallel.ForEach`, não de I/O assíncrono.
- O `ReadFile` sequencial e o `ReadFilesAConcurrent` paralelo compartilham a mesma lógica de processamento via `Process(file)`.