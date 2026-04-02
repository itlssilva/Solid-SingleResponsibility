# API de Notificações Multi-canal

## Single Responsibility (Responsabilidade Única)

_"Uma classe deve ter apenas um motivo para mudar."_

Uma classe não deve ser um "canivete suíço". Ela deve ser responsável por apenas uma funcionalidade ou parte lógica do sistema. Se uma classe faz muita coisa (ex: calcula imposto, gera PDF e envia e-mail), qualquer pequena mudança pode quebrar várias partes do código.

---

Cada serviço cuida de um único canal: e-mail, SMS, push ou webhook. Um orquestrador decide qual usar, mas nunca implementa o envio em si.

---

# Json Request

Exemplo

```json
{
  "recipient": "usuario@email.com",
  "subject": "Bem-vindo!",
  "body": "Olá, sua conta foi criada com sucesso.",
  "channel": "Email"
}
```

ou

```json
{
  "recipient": "usuario@email.com",
  "subject": "Bem-vindo!",
  "body": "Olá, sua conta foi criada com sucesso.",
  "channel": 0
}
```

Enum do channel

- 0 -> Email
- 1 -> Sms

## SendGrid e ApiKey

O canal de e-mail usa o SendGrid como provedor externo. A chave `ApiKey` nao fica versionada no repositorio e deve ser fornecida externamente em tempo de execucao.

Formato esperado da chave:

```text
SG.xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```

### Onde conseguir a ApiKey no futuro

Quando voce decidir habilitar o envio real de e-mails:

1. Acesse sua conta do SendGrid.
2. Abra `Settings` > `API Keys`.
3. Clique em `Create API Key`.
4. Dê um nome para a chave, por exemplo `NotificationsApiLocal`.
5. Escolha `Restricted Access`.
6. Habilite pelo menos a permissao `Mail Send`.
7. Crie a chave e copie o valor gerado.

Importante:

- O SendGrid mostra a chave completa apenas uma vez.
- Se perder a chave, sera necessario gerar outra.
- O `FromEmail` usado no projeto tambem precisa estar validado no SendGrid como remetente autorizado.

### Opcoes para armazenar a ApiKey sem colocar no repositorio

Opcao 1: `user-secrets` para desenvolvimento local

```powershell
dotnet user-secrets set "SendGrid:ApiKey" "SG.sua_chave_real_aqui" --project src\Notifications.Api\Notifications.Api.csproj
```

Para listar o valor configurado:

```powershell
dotnet user-secrets list --project src\Notifications.Api\Notifications.Api.csproj
```

Opcao 2: variavel de ambiente

```powershell
$env:SendGrid__ApiKey="SG.sua_chave_real_aqui"
```

Observacao:

- `FromEmail` e `FromName` ficam em `appsettings.json`.
- `ApiKey` deve vir de `user-secrets` ou variavel de ambiente.

### Como executar localmente

Com a implementacao atual, a aplicacao valida o bloco `SendGrid` na inicializacao. Isso significa que, sem `SendGrid:ApiKey`, a API nao sobe.

Quando a chave existir, execute:

```powershell
dotnet user-secrets set "SendGrid:ApiKey" "SG.sua_chave_real_aqui" --project src\Notifications.Api\Notifications.Api.csproj
dotnet run --project src\Notifications.Api\Notifications.Api.csproj
```

Ou usando variavel de ambiente:

```powershell
$env:SendGrid__ApiKey="SG.sua_chave_real_aqui"
dotnet run --project src\Notifications.Api\Notifications.Api.csproj
```

### Situacao atual do projeto

Se voce ainda nao vai criar uma chave agora, pode deixar essa etapa para o futuro. O codigo ja esta preparado para usar o SendGrid, mas para iniciar a API sera necessario configurar a `ApiKey` quando chegar esse momento.
