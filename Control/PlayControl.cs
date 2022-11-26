using UnityEngine;

namespace MaTech.Control {
    /// 这个接口会传给controller供有限条件下使用。
    /// 可以在多线程环境下使用，输入信息会被缓存到下一次IPlayController.UpdateControl结束后在主线程发出处理。
    public interface IPlayControl {
        void PlayKeyInput(KeyCode keyCode, bool isDown, int judgeTime);
        void PlayTouchInput(PlayInput.Finger finger, int judgeTime);
        void PlayIndexedInput(int index, bool isDown, int judgeTime);
    }
    
    /// 实现这个接口，便可以对游戏进行输入控制。
    public interface IPlayController {
        /// 是否为玩家控制，而非auto或replay。
        bool IsPlayer { get; }
        
        /// 启用输入，在输入启用期间，传入的control接口一直有效。
        void AttachController(IPlayControl playControl);
        /// 禁用输入，先前传入的control接口不再生效。
        void DetachController();
        
        /// 从指定的时间重新开始谱面回放或游玩。
        void ResetControl(int judgeTime);
        /// 在主线程的按帧更新，对于回放而言应当自上次的记录开始播放到小于等于judgeTime的位置。
        void UpdateControl(int judgeTime);
    }
}
