# SQL Server �����̳� ���� ���

### 1. SqlServer �̹��� �ٿ�ε�

```powershell
$sqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest"
docker pull $sqlServerImage
```

### 2. SqlServer �����̳� ����

```powershell
# ���� ����
$sqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest"
$hostPort = 1401
$containerPort = 1433
$userId = "sa"
$saPassword = "safepassword1!"
$containerName = "librarydb"
$databaseName = "librarydb"
$volumeName = "sqlvolume"
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=${saPassword}" -p ${hostPort}:${containerPort} --name ${containerName} -d -v ${volumeName}:/var/opt/mssql mcr.microsoft.com/mssql/server:2022-latest
```

### 3. Database ����

1. SQL Server �����̳� SA�������� ����
    
    ```powershell
    docker exec -it ${containerName} /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P ${saPassword}
    ```
    
2. �����ͺ��̽� ����
    
    ```sql
    CREATE DATABASE librarydb;
    GO
    EXIT
    ```
    

### 4. ConnectionString Ȯ��

1. ��Ŀ �����̳� ���� Bridge ���� IP Ȯ��
    
    ```powershell
    $sqlserverIpAddress = docker inspect -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' ${containerName} | Out-String
    $sqlserverIpAddress = $sqlserverIpAddress -replace [environment]::NewLine,""
    $sqlserverIpAddress = $sqlserverIpAddress.TrimStart("failed to get console mode for stdout: The handle is invalid.").Trim()
    echo "`n[SQL Server internal IP] `n${sqlserverIpAddress}`n"
    
    ``` 
    
    <aside>
    ?? ��� ����
    
    ```
    [SQL Server internal IP]
    172.17.0.2
    ```
    
    </aside>
    
2. ȣ��Ʈ���� ���� ������ ConnectionString ���
    
    ```powershell
    echo "`n[ConnectionString for HOST]`nData Source=localhost,${hostPort};Database=${databaseName};Integrated Security=false;User ID=${userId};Password='${saPassword}';Encrypt=true;TrustServerCertificate=true;`n"
    ```
    
    <aside>
    ?? ��� ����
    
    ```
    [ConnectionString for HOST]
    Data Source=localhost,1401;Database=librarydb;Integrated Security=false;User ID=;Password='';Encrypt=true;TrustServerCertificate=true;
    ```
    
    </aside>
    
3. �����̳ʰ� bridge���� ���� ������ ConnectionString ���
    
    ```powershell
    echo "`n[ConnectionString for container]`nData Source=${sqlserverIpAddress},${containerPort};Database=${databaseName};Integrated Security=false;User ID=${userId};Password='${saPassword}';Encrypt=true;TrustServerCertificate=true;`n"
    ```
    
    <aside>
    ?? ��� ����
    
    ```
    [ConnectionString for Container]
    Data Source=172.17.0.2,1433;Database=librarydb;Integrated Security=false;User ID=sa;Password='safepassword1!';Encrypt=true;TrustServerCertificate=true;
    ```
    
    </aside>