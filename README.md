# MMOGame_Client

Client는 Unity2D로 개발하였고 Server와 통신하여 캐릭터 이동 및 스킬 사용을 구현하고
실시간으로 다른 캐릭터의 변동사항을 전달 받아 클라이언트에 적용 가능하도록 하였습니다.


- 서버 통신
    
    Protohub를 사용하여  Packet을 구성하였습니다.
    
    Server, Client의 각 기능별  Packet을 생성하여 주고 받는 Packet의 클래스를 통해 기능을 구분하여 처리했습니다.
    
    S_ 태그 ⇒ Server에서 Client로 전송하는 Packet
    
    C_태그 ⇒ Client에서 Server로 전송하는 Packet 
    
    ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/7d0d7b4e-2aab-4c45-8093-a4fab9b5d493/545867f5-8d15-43d4-8da2-87c1678a4db5/image.png)