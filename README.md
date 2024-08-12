# MMOGame_Client

Client는 Unity2D로 개발하였고 Server와 통신하여 캐릭터 이동 및 스킬 사용을 구현하고
실시간으로 다른 캐릭터의 변동사항을 전달 받아 클라이언트에 적용 가능하도록 하였습니다.


- 서버 통신
    
    Protohub를 사용하여  Packet을 구성하였습니다.
    
    Server, Client의 각 기능별  Packet을 생성하여 주고 받는 Packet의 클래스를 통해 기능을 구분하여 처리했습니다.
    
    S_태그 ⇒ Server에서 Client로 전송하는 Packet
    
    C_태그 ⇒ Client에서 Server로 전송하는 Packet 


    
- Manager 기능

    자주 사용하는 주요 기능(Resource,Data,UI,Sound 등)을 싱글톤패턴으로 구성하여 활용함.

    Core과 Contents로 기능의 활용에 따라 구분하여 생성함.

    