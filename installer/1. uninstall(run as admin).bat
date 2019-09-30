@echo off
set PATH=%~dp0

schtasks /query /tn "nAdKakao"
schtasks /end /tn "nAdKakao"
schtasks /delete /tn "nAdKakao"
pause