@echo off
set PATH=%~dp0

schtasks /create /tn "nAdKakao" /tr "%~dp0nAdKaKao.exe" /sc ONLOGON
schtasks /run /tn "nAdKakao"
schtasks /query /tn "nAdKakao"
pause