
# ���� ����
$sqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest"
$hostPort = 1401
$containerPort = 1433
$userId = "sa"
$saPassword = "safepassword1!"
$containerName = "librarydb"
$databaseName = "librarydb"
$volumeName = "sqlvolume"

# ��Ŀ �����̳� ���� Bridge ���� IP Ȯ��
$sqlserverIpAddress = docker inspect -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' ${containerName} | Out-String
$sqlserverIpAddress = $sqlserverIpAddress -replace [environment]::NewLine,""
$sqlserverIpAddress = $sqlserverIpAddress.TrimStart("failed to get console mode for stdout: The handle is invalid.").Trim()
echo "`n[SQL Server internal IP] `n${sqlserverIpAddress}`n"

# ȣ��Ʈ���� ���� ������ ConnectionString ���
echo "`n[ConnectionString for HOST]`nData Source=localhost,${hostPort};Database=${databaseName};Integrated Security=false;User ID=${userId};Password='${saPassword}';Encrypt=true;TrustServerCertificate=true;`n"

# �����̳ʰ� bridge���� ���� ������ ConnectionString ���
echo "`n[ConnectionString for container]`nData Source=${sqlserverIpAddress},${containerPort};Database=${databaseName};Integrated Security=false;User ID=${userId};Password='${saPassword}';Encrypt=true;TrustServerCertificate=true;`n"
