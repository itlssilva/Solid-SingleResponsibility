## Plan: Adicionar coverage de testes

TL;DR - Adicionar um projeto de testes (xUnit), integrar Coverlet para coletar coverage e gerar relatório HTML com ReportGenerator; opcional: criar workflow GitHub Actions para executar testes e publicar relatórios.

**Steps**

1. Criar projeto de testes xUnit em `tests/Notifications.Tests` e adicionar referência para os projetos alvo (principalmente `Notifications.Application`).
2. Adicionar pacote `coverlet.collector` ao projeto de testes (_depends on step 1_).
3. (Opcional) Adicionar `coverlet.msbuild` e configurar geração de formato Cobertura, ou usar `--collect:"XPlat Code Coverage"` na linha de comando.
4. Adicionar `dotnet-reportgenerator-globaltool` como ferramenta global ou dependência de build para gerar relatório HTML a partir do arquivo Cobertura.
5. Criar um task/local script `dotnet test` que execute a coleta de coverage e gere o relatório no diretório `coverage-report`.
6. (Opcional) Criar workflow GitHub Actions em `.github/workflows/coverage.yml` para executar testes em CI e publicar artefato/relatório.

**Relevant files**

- `tests/Notifications.Tests/Notifications.Tests.csproj` — novo; adicionar referências e pacotes.
- `src/Notifications.Application/Notifications.Application.csproj` — referenciar no projeto de testes.
- Workspace tasks — opcionalmente adicionar `dotnet test` task no VS Code tasks.json.

**Verification**

1. Executar `dotnet test tests/Notifications.Tests/Notifications.Tests.csproj --collect:"XPlat Code Coverage"` e confirmar saída com `coverage.cobertura.xml` ou `coverage.info`.
2. Executar `reportgenerator` para gerar `coverage-report/index.html` e abrir no browser.
3. (CI) Validar GitHub Actions run e artefato publicado.

**Decisions / Assumptions**

- Usar xUnit por ser padrão em templates dotnet e compatível com Coverlet.
- Preferir `coverlet.collector` + `--collect:"XPlat Code Coverage"` para simplicidade; `coverlet.msbuild` é alternativa para integrar via msbuild args.

**Further Considerations**

1. Deseja que eu scaffold o projeto de testes e um exemplo de teste agora? (Recomendo sim, para facilitar validação)
2. Prefere gerar relatório HTML localmente ou configurar upload para serviço externo (Codecov/Coveralls)?
