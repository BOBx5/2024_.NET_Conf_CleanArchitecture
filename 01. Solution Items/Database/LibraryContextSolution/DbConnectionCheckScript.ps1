
# 변수 설정
$sqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest"
$hostPort = 1401
$containerPort = 1433
$userId = "sa"
$saPassword = "safepassword1!"
$containerName = "librarydb"
$databaseName = "librarydb"
$volumeName = "sqlvolume"

# 도커 컨테이너 내부 Bridge 접속 IP 확인
$sqlserverIpAddress = docker inspect -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' ${containerName} | Out-String
$sqlserverIpAddress = $sqlserverIpAddress -replace [environment]::NewLine,""
$sqlserverIpAddress = $sqlserverIpAddress.TrimStart("failed to get console mode for stdout: The handle is invalid.").Trim()
echo "`n[SQL Server internal IP] `n${sqlserverIpAddress}`n"

# 호스트에서 접근 가능한 ConnectionString 출력
echo "`n[ConnectionString for HOST]`nData Source=localhost,${hostPort};Database=${databaseName};Integrated Security=false;User ID=${userId};Password='${saPassword}';Encrypt=true;TrustServerCertificate=true;`n"

# 컨테이너간 bridge에서 접근 가능한 ConnectionString 출력
echo "`n[ConnectionString for container]`nData Source=${sqlserverIpAddress},${containerPort};Database=${databaseName};Integrated Security=false;User ID=${userId};Password='${saPassword}';Encrypt=true;TrustServerCertificate=true;`n"
