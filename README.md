#### NuGet包：
#### microsoft.entityframeworkcore\7.0.15
#### microsoft.entityframeworkcore.design\7.0.15
#### microsoft.entityframeworkcore.tools\7.0.15
#### pomelo.entityframeworkcore.mysql\7.0.0
#### microsoft.extensions.configuration.json\7.0.0
####
#### 在管理用户机密（secret.json）配置 ConnectionStrings:Database_EM , HashSalt:Default , SSLCertificate:Path,Password 节
####
#### 使用DB First模式：把数据库的表，转为Models
#### 程序包管理控制台执行：
#### Scaffold-DbContext Name=ConnectionStrings:Database_EM Pomelo.EntityFrameworkCore.MySql -o Models
#### 数据库表更新时，使用-Force覆盖现有文件
#### 
#### （
##### 转SQLServer：
##### 类DbEmContext修改：optionsBuilder.UseSqlServer("name=ConnectionStrings:Database_EM");
##### "ConnectionStrings:Database_EM": "Server=DESKTOP-XXXXXXX\SQLEXPRESS;Database=db_em;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
##### 项目目录下PowerShell执行
##### dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 7.0.15
##### dotnet ef migrations add InitialCreate
##### Migrations下对应类修改：PRIMARY加唯一标识(如表名后缀)、datetime(3)改datetime、bigint(20)改bigint、int(1)改int
##### 建数据库db_em
##### dotnet ef database update
##### 手动补建视图
#### ）
####
#### 自签名pfx证书生成
#### $cert = New-SelfSignedCertificate -DnsName "localhost" -CertStoreLocation "C:\Users\Administrator\Desktop\SSL" -Provider "Microsoft RSA SChannel Cryptographic Provider"
#### $password = ConvertTo-SecureString -String "YourPassword" -Force -AsPlainText
#### Export-PfxCertificate -Cert "cert:\LocalMachine\My\$($cert.Thumbprint)" -FilePath "C:\Users\Administrator\Desktop\SSL\cert.pfx" -Password $password
