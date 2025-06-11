# EM-Server

## 项目要求
- .NET 6 WebAPI项目
- 需要安装.NET 6或以上构建工具和运行时

## 依赖项
### NuGet包
- Microsoft.EntityFrameworkCore (7.0.15)
- Microsoft.EntityFrameworkCore.Design (7.0.15)
- Microsoft.EntityFrameworkCore.Tools (7.0.15)
- Microsoft.EntityFrameworkCore.SqlServer (7.0.15)
- Microsoft.Extensions.Configuration.Json (7.0.0)

## 配置说明
在管理用户机密（secret.json）中配置以下内容：
- ConnectionStrings:Database_EM
- HashSalt:Default
- SSLCertificate:Path
- SSLCertificate:Password

## 数据库设置
### 采用Code First模式，配置步骤
1. 在项目目录下PowerShell执行：
   ```powershell
   dotnet ef migrations add InitialCreate
   ```

2. 创建数据库：
   - 数据库名称：db_em
   - ~~"对于新数据库：右键单击"数据库"，然后选择"新建数据库"。如果不希望使用默认排序规则，则选择"选项"页，然后从"排序规则"下拉列表中选择某一排序规则。"~~

3. 在项目目录下PowerShell执行：
   ```powershell
   dotnet ef database update
   ```

4. 设置消息表自增Id起始值：
   ```sql
   DBCC CHECKIDENT ('t_private_message', RESEED, 1000000000000001024);
   ```

5. ~~"UTF-8受char和varchar数据类型支持，并在创建对象的排序规则或将其更改为带有UTF8后缀的排序规则时启用。例如，将LATIN1_GENERAL_100_CI_AS_SC更改为LATIN1_GENERAL_100_CI_AS_SC_UTF8。"~~

6. 手动补建视图

## 部署要求
发布后需要在部署环境安装：
- windowsdesktop-runtime-6.0.36-win-x64
- aspnetcore-runtime-6.0.36-win-x64

## SSL证书配置
### 自签名pfx证书生成步骤
在PowerShell中执行以下命令：
```powershell
$cert = New-SelfSignedCertificate -DnsName "localhost" -CertStoreLocation "C:\Users\Administrator\Desktop\SSL" -Provider "Microsoft RSA SChannel Cryptographic Provider"
$password = ConvertTo-SecureString -String "YourPassword" -Force -AsPlainText
Export-PfxCertificate -Cert "cert:\LocalMachine\My\$($cert.Thumbprint)" -FilePath "C:\Users\Administrator\Desktop\SSL\cert.pfx" -Password $password
```
