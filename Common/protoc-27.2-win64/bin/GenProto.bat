protoc.exe -I=./ --csharp_out=./ ./Protocol.proto 
IF ERRORLEVEL 1 PAUSE

START ../../../../MMOGame_Server/PacketGenerator/bin/Debug/net8.0/PacketGenerator.exe ./Protocol.proto
XCOPY /Y Protocol.cs "../../../Assets/Scripts/Packet"
XCOPY /Y Protocol.cs "../../../../MMOGame_Server/Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../../Assets/Scripts/Packet"
XCOPY /Y ServerPacketManager.cs "../../../../MMOGame_Server/Server/Packet"