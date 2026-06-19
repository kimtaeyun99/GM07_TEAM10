// 💡 인터페이스 이름은 관례상 맨 앞에 대문자 'I'를 붙입니다.
public interface ITakable
{
    // 이 인터페이스를 사용하는 클래스는 반드시 TakeItem() 기능을 만들어야 합니다.
    void TakeItem();
}