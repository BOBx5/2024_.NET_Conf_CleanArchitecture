# SQL Server 컨테이너 생성 방법

### 1. SqlServer 이미지 다운로드

```powershell
$sqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest"
docker pull $sqlServerImage
```

### 2. SqlServer 컨테이너 생성

```powershell
# 변수 설정
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

### 3. Database 생성

1. SQL Server 컨테이너 SA계정으로 접속
    
    ```powershell
    docker exec -it ${containerName} /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P ${saPassword}
    ```
    
2. 데이터베이스 생성
    
    ```sql
    CREATE DATABASE librarydb;
    GO
    EXIT
    ```
    

### 4. ConnectionString 확인

1. 도커 컨테이너 내부 Bridge 접속 IP 확인
    
    ```powershell
    $sqlserverIpAddress = docker inspect -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' ${containerName} | Out-String
    $sqlserverIpAddress = $sqlserverIpAddress -replace [environment]::NewLine,""
    $sqlserverIpAddress = $sqlserverIpAddress.TrimStart("failed to get console mode for stdout: The handle is invalid.").Trim()
    echo "`n[SQL Server internal IP] `n${sqlserverIpAddress}`n"
    
    ``` 
    
    <aside>
    ?? 출력 예시
    
    ```
    [SQL Server internal IP]
    172.17.0.2
    ```
    
    </aside>
    
2. 호스트에서 접근 가능한 ConnectionString 출력
    
    ```powershell
    echo "`n[ConnectionString for HOST]`nData Source=localhost,${hostPort};Database=${databaseName};Integrated Security=false;User ID=${userId};Password='${saPassword}';Encrypt=true;TrustServerCertificate=true;`n"
    ```
    
    <aside>
    ?? 출력 예시
    
    ```
    [ConnectionString for HOST]
    Data Source=localhost,1401;Database=librarydb;Integrated Security=false;User ID=;Password='';Encrypt=true;TrustServerCertificate=true;
    ```
    
    </aside>
    
3. 컨테이너간 bridge에서 접근 가능한 ConnectionString 출력
    
    ```powershell
    echo "`n[ConnectionString for container]`nData Source=${sqlserverIpAddress},${containerPort};Database=${databaseName};Integrated Security=false;User ID=${userId};Password='${saPassword}';Encrypt=true;TrustServerCertificate=true;`n"
    ```
    
    <aside>
    ?? 출력 예시
    
    ```
    [ConnectionString for Container]
    Data Source=172.17.0.2,1433;Database=librarydb;Integrated Security=false;User ID=sa;Password='safepassword1!';Encrypt=true;TrustServerCertificate=true;
    ```
    
    </aside>