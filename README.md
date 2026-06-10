# Corevent

Aplicativo mobile de gerenciamento de eventos. Explore eventos, compre ingressos, crie e gerencie seus próprios eventos, e faça check-in via QR Code.

## Stack

- **Framework:** .NET MAUI (Android + Windows)
- **Padrão:** MVVM com `CommunityToolkit.Mvvm` (source generators)
- **Navegação:** Shell com TabBar + rotas de detalhe
- **API:** REST — [Corevent API](https://github.com/corevent/api-corevent)
- **Pagamentos:** PagBank (checkout + webhook)
- **QR Code:** ZXing.Net.Maui (leitura) + QRCoder (geração)

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Android SDK (API 21+) — para build Android
- Windows SDK (10.0.17763+) — para build Windows

## Executar

```bash
# Windows
dotnet run -f net10.0-windows10.0.19041.0

# Android (emulador ou dispositivo)
dotnet build -t:Run -f net10.0-android
```

## Build

```bash
dotnet build
```

## Targets

| Plataforma | SDK mínimo |
|------------|-----------|
| Android    | API 21    |
| Windows    | 10.0.17763 |

## Funcionalidades

### Usuário
- Cadastro com verificação de email
- Login / Logout
- Recuperação de senha
- Edição de perfil
- Favoritar eventos
- Avaliar eventos (1-5 estrelas)

### Explorar
- Busca textual com debounce
- Filtro por categoria (Música, Tecnologia, Esportes, etc.)
- Paginação infinita
- Destaques na Home (score por proximidade, participantes, tipo)

### Organizador
- Criar evento em 3 etapas (informações, data, localização)
- Editor de evento com PATCH parcial
- Publicar / Cancelar / Excluir evento
- Gerenciar tipos de ingresso (preço, quantidade, período)
- Gerenciar atrações/grade
- Gerenciar equipe (staff + convites)
- Check-in via leitura de QR Code

### Ingressos
- Lista de ingressos (próximos / passados)
- QR Code do ingresso
- Checkout com PagBank (redirect + deep link de retorno)
- Histórico de pedidos com detalhes

### Colaborador
- Painel de eventos como staff
- Detalhe do evento colaborador
- Check-in (se tiver permissão)

## Deep Link

O app registra o esquema `corevent://` para retorno do PagBank:

```
corevent://orders
```

Após o pagamento, o usuário é redirecionado automaticamente para a tab **Ingressos**.

## Publicar pacote MSIX (Windows)

Para que deep links funcionem no Windows, o app precisa ser **empacotado e instalado** via MSIX (o `dotnet run` não registra o protocolo `corevent://`).

### 1. Gerar certificado de assinatura

```powershell
New-SelfSignedCertificate -Type Custom `
  -Subject "CN=CoreventFatec" `
  -KeyUsage DigitalSignature `
  -FriendlyName "Corevent cert" `
  -CertStoreLocation "Cert:\CurrentUser\My" `
  -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3", "2.5.29.19={text}")
```

Copie a **impressão digital** (Thumbprint) do certificado gerado.

### 2. Atualizar a impressão digital no `.csproj`

Em `CoreventApp.csproj`, substitua `PackageCertificateThumbprint` pela sua impressão digital:

```xml
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows' and '$(Configuration)' == 'Release'">
  <WindowsPackageType>MSIX</WindowsPackageType>
  <AppxPackageSigningEnabled>true</AppxPackageSigningEnabled>
  <PackageCertificateThumbprint>SEU_THUMBPRINT_AQUI</PackageCertificateThumbprint>
</PropertyGroup>
```

### 3. Publicar

```bash
dotnet publish -f net10.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win-x64
```

O `.msix` será gerado em:
```
bin\Release\net10.0-windows10.0.19041.0\win-x64\AppPackages\CoreventApp_1.0.0.0_Test\
```

### 4. Instalar

Antes de instalar, confie no certificado:

1. Clique com o botão direito no `.msix` → **Propriedades**
2. Guia **Assinaturas Digitais** → selecione o certificado → **Detalhes**
3. **Exibir Certificado** → **Instalar Certificado...**
4. Escolha **Computador Local** → **Avançar**
5. Selecione **Colocar todos os certificados no repositório a seguir**
6. **Procurar...** → **Pessoas Confiáveis** → **OK**
7. **Avançar** → **Concluir**

Agora abra o `.msix` e clique em **Instalar**.

> **Nota:** Um certificado autoassinado só funciona em máquinas que o trustam. Para distribuição pública, use um certificado de uma AC confiável.

## Licença

MIT
