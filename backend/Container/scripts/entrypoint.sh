#!/bin/bash
echo "Starting SQLServer"
/opt/mssql/bin/sqlservr &  
echo "Waiting for the initializating of SQLServer"
sleep 10
echo "Executing SQL Script"
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P 'p@ssw0rd' -i /db.sql -C
wait