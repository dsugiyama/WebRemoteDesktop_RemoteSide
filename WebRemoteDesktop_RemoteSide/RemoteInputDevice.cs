using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace WebRemoteDesktop_RemoteSide
{
    class RemoteInputDevice
    {
        public RemoteInputDevice()
        {

        }
        //参考サイト
        /* https://msdn.microsoft.com/ja-jp/library/ms646273.aspx
         * http://homepage2.nifty.com/nonnon/SoftSample/CS.NET/SampleSendInput.html
         * http://homepage3.nifty.com/midori_no_bike/CS/userIO.html
         * 
         * */


        //カーソル移動
        [DllImport("User32.Dll", EntryPoint = "SetCursorPos")]//この記述の直下の関数を実行したときに、このDLLが呼び出される
        public static extern int setCursor(int x, int y);

        //マウスクリック
        [DllImport("User32.Dll", EntryPoint = "SetCursorPos")]//この記述の直下の関数を実行したときに、このDLLが呼び出される
        public static extern int mouse_event();
        
        //キーボード
        
        //===========================================================================================
        [DllImport("user32.dll")]
        private extern static uint SendInput(
            uint nInputs,   // INPUT 構造体の数(イベント数)
            INPUT[] pInputs,   // INPUT 構造体
            int cbSize     // INPUT 構造体のサイズ
        );

        [StructLayout(LayoutKind.Sequential)]  // アンマネージ DLL 対応用 struct 記述宣言
        struct INPUT
        {
            public int inputType;  // 0 = INPUT_MOUSE(デフォルト), 1 = INPUT_KEYBOARD
            public InputUnion inputUnion;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct InputUnion //インプット共用体（複数の型をもつ構造体が使える）
        {
            [FieldOffset(0)]
            internal MouseInput mouseInput;
            [FieldOffset(0)]
            internal KeyboardInput keyboardInput;
            [FieldOffset(0)]
            internal HardwareInput hardwareInput;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MouseInput
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct KeyboardInput
        {
            public VirtualKeyShort wVk;
            public ScanCodeShort wScan;
            public KEYEVENTF dwFlags;
            public int time;
            public UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HardwareInput//いらないかもしれない
        {
            internal int uMsg;
            internal short wParamL;
            internal short wParamH;
        }


        public class MouseAction//マウスの動作 Enumだと一度Intに変換しないといけないため、Classで実装した。
        {
           //constをつけないとクラスを作成外部から参照できない
           public const int MOUSEEVENTF_MOVED = 0x0001;
           public const int MOUSEEVENTF_LEFTDOWN = 0x0002;  // 左ボタン Down
           public const int MOUSEEVENTF_LEFTUP = 0x0004;  // 左ボタン Up
           public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;  // 右ボタン Down
           public const int MOUSEEVENTF_RIGHTUP = 0x0010;  // 右ボタン Up
           public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;  // 中ボタン Down
           public const int MOUSEEVENTF_MIDDLEUP = 0x0040;  // 中ボタン Up
           public const int MOUSEEVENTF_WHEEL = 0x0080;
           public const int MOUSEEVENTF_XDOWN = 0x0100;
           public const int MOUSEEVENTF_XUP = 0x0200;
           public const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        }

        public enum mouseButton
        {
            left,right,middle
        }

        //http://stackoverflow.com/questions/20482338/simulate-keyboard-input-in-c-sharp
        //http://stackoverflow.com/questions/20482338/simulate-keyboard-input-in-c-sharp
        internal enum VirtualKeyShort : short
        {

            [Description("Left mouse button")]
            VK_LBUTTON = 0x01,

            [Description("Right mouse button")]
            VK_RBUTTON = 0x02,

            [Description("Control-break processing")]
            VK_CANCEL = 0x03,

            [Description("Middle mouse button (three-button mouse)")]
            VK_MBUTTON = 0x04,

            [Description("X1 mouse button")]
            VK_XBUTTON1 = 0x05,

            [Description("X2 mouse button")]
            VK_XBUTTON2 = 0x06,

            [Description("BACKSPACE key")]
            VK_BACK = 0x08,

            [Description("TAB key")]
            VK_TAB = 0x09,

            [Description("CLEAR key")]
            VK_CLEAR = 0x0C,

            [Description("ENTER key")]
            VK_RETURN = 0x0D,

            [Description("SHIFT key")]
            VK_SHIFT = 0x10,

            [Description("CTRL key")]
            VK_CONTROL = 0x11,

            [Description("ALT key")]
            VK_MENU = 0x12,

            [Description("PAUSE key")]
            VK_PAUSE = 0x13,

            [Description("CAPS LOCK key")]
            VK_CAPITAL = 0x14,

            [Description("IME Kana mode")]
            VK_KANA = 0x15,

            [Description("IME Hanguel mode (maintained for compatibility; use VK_HANGUL)")]
            VK_HANGUEL = 0x15,

            [Description("IME Hangul mode")]
            VK_HANGUL = 0x15,

            [Description("IME Junja mode")]
            VK_JUNJA = 0x17,

            [Description("IME final mode")]
            VK_FINAL = 0x18,

            [Description("IME Hanja mode")]
            VK_HANJA = 0x19,

            [Description("IME Kanji mode")]
            VK_KANJI = 0x19,

            [Description("ESC key")]
            VK_ESCAPE = 0x1B,

            [Description("IME convert")]
            VK_CONVERT = 0x1C,

            [Description("IME nonconvert")]
            VK_NONCONVERT = 0x1D,

            [Description("IME accept")]
            VK_ACCEPT = 0x1E,

            [Description("IME mode change request")]
            VK_MODECHANGE = 0x1F,

            [Description("SPACEBAR")]
            VK_SPACE = 0x20,

            [Description("PAGE UP key")]
            VK_PRIOR = 0x21,

            [Description("PAGE DOWN key")]
            VK_NEXT = 0x22,

            [Description("END key")]
            VK_END = 0x23,

            [Description("HOME key")]
            VK_HOME = 0x24,

            [Description("LEFT ARROW key")]
            VK_LEFT = 0x25,

            [Description("UP ARROW key")]
            VK_UP = 0x26,

            [Description("RIGHT ARROW key")]
            VK_RIGHT = 0x27,

            [Description("DOWN ARROW key")]
            VK_DOWN = 0x28,

            [Description("SELECT key")]
            VK_SELECT = 0x29,

            [Description("PRINT key")]
            VK_PRINT = 0x2A,

            [Description("EXECUTE key")]
            VK_EXECUTE = 0x2B,

            [Description("PRINT SCREEN key")]
            VK_SNAPSHOT = 0x2C,

            [Description("INS key")]
            VK_INSERT = 0x2D,

            [Description("DEL key")]
            VK_DELETE = 0x2E,

            [Description("HELP key")]
            VK_HELP = 0x2F,

            [Description("0 key")]
            K_0 = 0x30,

            [Description("1 key")]
            K_1 = 0x31,

            [Description("2 key")]
            K_2 = 0x32,

            [Description("3 key")]
            K_3 = 0x33,

            [Description("4 key")]
            K_4 = 0x34,

            [Description("5 key")]
            K_5 = 0x35,

            [Description("6 key")]
            K_6 = 0x36,

            [Description("7 key")]
            K_7 = 0x37,

            [Description("8 key")]
            K_8 = 0x38,

            [Description("9 key")]
            K_9 = 0x39,

            [Description("A key")]
            K_A = 0x41,

            [Description("B key")]
            K_B = 0x42,

            [Description("C key")]
            K_C = 0x43,

            [Description("D key")]
            K_D = 0x44,

            [Description("E key")]
            K_E = 0x45,

            [Description("F key")]
            K_F = 0x46,

            [Description("G key")]
            K_G = 0x47,

            [Description("H key")]
            K_H = 0x48,

            [Description("I key")]
            K_I = 0x49,

            [Description("J key")]
            K_J = 0x4A,

            [Description("K key")]
            K_K = 0x4B,

            [Description("L key")]
            K_L = 0x4C,

            [Description("M key")]
            K_M = 0x4D,

            [Description("N key")]
            K_N = 0x4E,

            [Description("O key")]
            K_O = 0x4F,

            [Description("P key")]
            K_P = 0x50,

            [Description("Q key")]
            K_Q = 0x51,

            [Description("R key")]
            K_R = 0x52,

            [Description("S key")]
            K_S = 0x53,

            [Description("T key")]
            K_T = 0x54,

            [Description("U key")]
            K_U = 0x55,

            [Description("V key")]
            K_V = 0x56,

            [Description("W key")]
            K_W = 0x57,

            [Description("X key")]
            K_X = 0x58,

            [Description("Y key")]
            K_Y = 0x59,

            [Description("Z key")]
            K_Z = 0x5A,

            [Description("Left Windows key (Natural keyboard)")]
            VK_LWIN = 0x5B,

            [Description("Right Windows key (Natural keyboard)")]
            VK_RWIN = 0x5C,

            [Description("Applications key (Natural keyboard)")]
            VK_APPS = 0x5D,

            [Description("Computer Sleep key")]
            VK_SLEEP = 0x5F,

            [Description("Numeric keypad 0 key")]
            VK_NUMPAD0 = 0x60,

            [Description("Numeric keypad 1 key")]
            VK_NUMPAD1 = 0x61,

            [Description("Numeric keypad 2 key")]
            VK_NUMPAD2 = 0x62,

            [Description("Numeric keypad 3 key")]
            VK_NUMPAD3 = 0x63,

            [Description("Numeric keypad 4 key")]
            VK_NUMPAD4 = 0x64,

            [Description("Numeric keypad 5 key")]
            VK_NUMPAD5 = 0x65,

            [Description("Numeric keypad 6 key")]
            VK_NUMPAD6 = 0x66,

            [Description("Numeric keypad 7 key")]
            VK_NUMPAD7 = 0x67,

            [Description("Numeric keypad 8 key")]
            VK_NUMPAD8 = 0x68,

            [Description("Numeric keypad 9 key")]
            VK_NUMPAD9 = 0x69,

            [Description("Multiply key")]
            VK_MULTIPLY = 0x6A,

            [Description("Add key")]
            VK_ADD = 0x6B,

            [Description("Separator key")]
            VK_SEPARATOR = 0x6C,

            [Description("Subtract key")]
            VK_SUBTRACT = 0x6D,

            [Description("Decimal key")]
            VK_DECIMAL = 0x6E,

            [Description("Divide key")]
            VK_DIVIDE = 0x6F,

            [Description("F1 key")]
            VK_F1 = 0x70,

            [Description("F2 key")]
            VK_F2 = 0x71,

            [Description("F3 key")]
            VK_F3 = 0x72,

            [Description("F4 key")]
            VK_F4 = 0x73,

            [Description("F5 key")]
            VK_F5 = 0x74,

            [Description("F6 key")]
            VK_F6 = 0x75,

            [Description("F7 key")]
            VK_F7 = 0x76,

            [Description("F8 key")]
            VK_F8 = 0x77,

            [Description("F9 key")]
            VK_F9 = 0x78,

            [Description("F10 key")]
            VK_F10 = 0x79,

            [Description("F11 key")]
            VK_F11 = 0x7A,

            [Description("F12 key")]
            VK_F12 = 0x7B,

            [Description("F13 key")]
            VK_F13 = 0x7C,

            [Description("F14 key")]
            VK_F14 = 0x7D,

            [Description("F15 key")]
            VK_F15 = 0x7E,

            [Description("F16 key")]
            VK_F16 = 0x7F,

            [Description("F17 key")]
            VK_F17 = 0x80,

            [Description("F18 key")]
            VK_F18 = 0x81,

            [Description("F19 key")]
            VK_F19 = 0x82,

            [Description("F20 key")]
            VK_F20 = 0x83,

            [Description("F21 key")]
            VK_F21 = 0x84,

            [Description("F22 key")]
            VK_F22 = 0x85,

            [Description("F23 key")]
            VK_F23 = 0x86,

            [Description("F24 key")]
            VK_F24 = 0x87,

            [Description("NUM LOCK key")]
            VK_NUMLOCK = 0x90,

            [Description("SCROLL LOCK key")]
            VK_SCROLL = 0x91,

            [Description("Left SHIFT key")]
            VK_LSHIFT = 0xA0,

            [Description("Right SHIFT key")]
            VK_RSHIFT = 0xA1,

            [Description("Left CONTROL key")]
            VK_LCONTROL = 0xA2,

            [Description("Right CONTROL key")]
            VK_RCONTROL = 0xA3,

            [Description("Left MENU key")]
            VK_LMENU = 0xA4,

            [Description("Right MENU key")]
            VK_RMENU = 0xA5,

            [Description("Browser Back key")]
            VK_BROWSER_BACK = 0xA6,

            [Description("Browser Forward key")]
            VK_BROWSER_FORWARD = 0xA7,

            [Description("Browser Refresh key")]
            VK_BROWSER_REFRESH = 0xA8,

            [Description("Browser Stop key")]
            VK_BROWSER_STOP = 0xA9,

            [Description("Browser Search key")]
            VK_BROWSER_SEARCH = 0xAA,

            [Description("Browser Favorites key")]
            VK_BROWSER_FAVORITES = 0xAB,

            [Description("Browser Start and Home key")]
            VK_BROWSER_HOME = 0xAC,

            [Description("Volume Mute key")]
            VK_VOLUME_MUTE = 0xAD,

            [Description("Volume Down key")]
            VK_VOLUME_DOWN = 0xAE,

            [Description("Volume Up key")]
            VK_VOLUME_UP = 0xAF,

            [Description("Next Track key")]
            VK_MEDIA_NEXT_TRACK = 0xB0,

            [Description("Previous Track key")]
            VK_MEDIA_PREV_TRACK = 0xB1,

            [Description("Stop Media key")]
            VK_MEDIA_STOP = 0xB2,

            [Description("Play/Pause Media key")]
            VK_MEDIA_PLAY_PAUSE = 0xB3,

            [Description("Start Mail key")]
            VK_LAUNCH_MAIL = 0xB4,

            [Description("Select Media key")]
            VK_LAUNCH_MEDIA_SELECT = 0xB5,

            [Description("Start Application 1 key")]
            VK_LAUNCH_APP1 = 0xB6,

            [Description("Start Application 2 key")]
            VK_LAUNCH_APP2 = 0xB7,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the ';:' key")]
            VK_OEM_1 = 0xBA,

            [Description("For any country/region, the '+' key")]
            VK_OEM_PLUS = 0xBB,

            [Description("For any country/region, the ',' key")]
            VK_OEM_COMMA = 0xBC,

            [Description("For any country/region, the '-' key")]
            VK_OEM_MINUS = 0xBD,

            [Description("For any country/region, the '.' key")]
            VK_OEM_PERIOD = 0xBE,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '/?' key")]
            VK_OEM_2 = 0xBF,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '`~' key")]
            VK_OEM_3 = 0xC0,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '[{' key")]
            VK_OEM_4 = 0xDB,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '\\|' key")]
            VK_OEM_5 = 0xDC,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the ']}' key")]
            VK_OEM_6 = 0xDD,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the 'single-quote/double-quote' key")]
            VK_OEM_7 = 0xDE,

            [Description("Used for miscellaneous characters; it can vary by keyboard.")]
            VK_OEM_8 = 0xDF,


            [Description("Either the angle bracket key or the backslash key on the RT 102-key keyboard")]
            VK_OEM_102 = 0xE2,

            [Description("IME PROCESS key")]
            VK_PROCESSKEY = 0xE5,


            [Description("Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP")]
            VK_PACKET = 0xE7,

            [Description("Attn key")]
            VK_ATTN = 0xF6,

            [Description("CrSel key")]
            VK_CRSEL = 0xF7,

            [Description("ExSel key")]
            VK_EXSEL = 0xF8,

            [Description("Erase EOF key")]
            VK_EREOF = 0xF9,

            [Description("Play key")]
            VK_PLAY = 0xFA,

            [Description("Zoom key")]
            VK_ZOOM = 0xFB,

            [Description("PA1 key")]
            VK_PA1 = 0xFD,

            [Description("Clear key")]
            VK_OEM_CLEAR = 0xFE,

        }

        internal enum ScanCodeShort : short
        {
            LBUTTON = 0,
            RBUTTON = 0,
            CANCEL = 70,
            MBUTTON = 0,
            XBUTTON1 = 0,
            XBUTTON2 = 0,
            BACKSPACE = 14,
            TAB = 15,
            CLEAR = 76,
            ENTER = 28,
            SHIFT = 42,
            CONTROL = 29,
            MENU = 56,
            PAUSE = 0,
            CAPITAL = 58,
            KANA = 0,
            HANGUL = 0,
            JUNJA = 0,
            FINAL = 0,
            HANJA = 0,
            KANJI = 0,
            ESCAPE = 1,
            CONVERT = 0,
            NONCONVERT = 0,
            ACCEPT = 0,
            MODECHANGE = 0,
            SPACE = 57,
            PRIOR = 73,
            NEXT = 81,
            END = 79,
            HOME = 71,
            LEFT = 75,
            UP = 72,
            RIGHT = 77,
            DOWN = 80,
            SELECT = 0,
            PRINT = 0,
            EXECUTE = 0,
            SNAPSHOT = 84,
            INSERT = 82,
            DELETE = 83,
            HELP = 99,
            KEY_0 = 11,
            KEY_1 = 2,
            KEY_2 = 3,
            KEY_3 = 4,
            KEY_4 = 5,
            KEY_5 = 6,
            KEY_6 = 7,
            KEY_7 = 8,
            KEY_8 = 9,
            KEY_9 = 10,
            KEY_A = 30,
            KEY_B = 48,
            KEY_C = 46,
            KEY_D = 32,
            KEY_E = 18,
            KEY_F = 33,
            KEY_G = 34,
            KEY_H = 35,
            KEY_I = 23,
            KEY_J = 36,
            KEY_K = 37,
            KEY_L = 38,
            KEY_M = 50,
            KEY_N = 49,
            KEY_O = 24,
            KEY_P = 25,
            KEY_Q = 16,
            KEY_R = 19,
            KEY_S = 31,
            KEY_T = 20,
            KEY_U = 22,
            KEY_V = 47,
            KEY_W = 17,
            KEY_X = 45,
            KEY_Y = 21,
            KEY_Z = 44,
            LWIN = 91,
            RWIN = 92,
            APPS = 93,
            SLEEP = 95,
            NUMPAD0 = 82,
            NUMPAD1 = 79,
            NUMPAD2 = 80,
            NUMPAD3 = 81,
            NUMPAD4 = 75,
            NUMPAD5 = 76,
            NUMPAD6 = 77,
            NUMPAD7 = 71,
            NUMPAD8 = 72,
            NUMPAD9 = 73,
            MULTIPLY = 55,
            ADD = 78,
            SEPARATOR = 0,
            SUBTRACT = 74,
            DECIMAL = 83,
            DIVIDE = 53,
            F1 = 59,
            F2 = 60,
            F3 = 61,
            F4 = 62,
            F5 = 63,
            F6 = 64,
            F7 = 65,
            F8 = 66,
            F9 = 67,
            F10 = 68,
            F11 = 87,
            F12 = 88,
            F13 = 100,
            F14 = 101,
            F15 = 102,
            F16 = 103,
            F17 = 104,
            F18 = 105,
            F19 = 106,
            F20 = 107,
            F21 = 108,
            F22 = 109,
            F23 = 110,
            F24 = 118,
            NUMLOCK = 69,
            SCROLL = 70,
            LSHIFT = 42,
            RSHIFT = 54,
            LCONTROL = 29,
            RCONTROL = 29,
            LMENU = 56,
            RMENU = 56,
            /*BROWSER_BACK = 106,
            BROWSER_FORWARD = 105,
            BROWSER_REFRESH = 103,
            BROWSER_STOP = 104,
            BROWSER_SEARCH = 101,
            BROWSER_FAVORITES = 102,
            BROWSER_HOME = 50,
            VOLUME_MUTE = 32,
            VOLUME_DOWN = 46,
            VOLUME_UP = 48,
            MEDIA_NEXT_TRACK = 25,
            MEDIA_PREV_TRACK = 16,
            MEDIA_STOP = 36,
            MEDIA_PLAY_PAUSE = 34,
            LAUNCH_MAIL = 108,
            LAUNCH_MEDIA_SELECT = 109,
            LAUNCH_APP1 = 107,
            LAUNCH_APP2 = 33,
            OEM_1 = 39,
            OEM_PLUS = 13,
            OEM_COMMA = 51,
            OEM_MINUS = 12,
            OEM_PERIOD = 52,
            OEM_2 = 53,
            OEM_3 = 41,
            OEM_4 = 26,
            OEM_5 = 43,
            OEM_6 = 27,
            OEM_7 = 40,
            OEM_8 = 0,
            OEM_102 = 86,
            PROCESSKEY = 0,
            PACKET = 0,
            ATTN = 0,
            CRSEL = 0,
            EXSEL = 0,
            EREOF = 93,
            PLAY = 0,
            ZOOM = 98,
            NONAME = 0,
            PA1 = 0,
            OEM_CLEAR = 0,*/
        }

        [Flags]
        internal enum KEYEVENTF : uint
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            SCANCODE = 0x0008,
            UNICODE = 0x0004
        }


        //===========================================================================================

        //クラス内外の関数から利用される関数===========================================================================================

        public void setMouseCursor(int x,int y)
        {
            //現在のカーソル位置と引数に指定されたカーソル位置が異なる場合のみ、マウスカーソルの移動を行う
            if (x != System.Windows.Forms.Cursor.Position.X || y != System.Windows.Forms.Cursor.Position.Y)
            {
                //x及びyのいずれかの引数が与えられていない場合、現在のカーソル位置を設定する
                if (x == -1 || y == -1)
                {
                    x = System.Windows.Forms.Cursor.Position.X;
                    y = System.Windows.Forms.Cursor.Position.Y;
                }

                //マウスカーソルを移動
                setCursor(x, y);
            }
        }

        private void mouseClick(int x,int y,mouseButton mouseButton)
        {
            //現在のカーソル位置と引数に指定されたカーソル位置が異なる場合のみ、マウスカーソルの移動を行う
            setMouseCursor(x, y);
            
            //指定されたボタンをクリックする     
            INPUT[] input = new INPUT[2];

            switch (mouseButton)
            {
                case mouseButton.left:
                    input[0].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_LEFTDOWN;
                    input[1].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_LEFTUP;
                    break;

                case mouseButton.right:
                    input[0].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_RIGHTDOWN;
                    input[1].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_RIGHTUP;
                    break;

                case mouseButton.middle:
                    input[0].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_MIDDLEDOWN;
                    input[1].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_MIDDLEUP;
                    break;
                    
                default:
                    break;
            }
          
            SendInput(2, input, Marshal.SizeOf(input[0]));
        }

        private void mouseDown(int x, int y, mouseButton mouseButton)
        {
            //現在のカーソル位置と引数に指定されたカーソル位置が異なる場合のみ、マウスカーソルの移動を行う
            setMouseCursor(x, y);

            //指定されたボタンをクリックする     
            INPUT[] input = new INPUT[1];

            switch (mouseButton)
            {
                case mouseButton.left:
                    input[0].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_LEFTDOWN;
                    break;

                case mouseButton.right:
                    input[0].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_RIGHTDOWN;
                    break;

                case mouseButton.middle:
                    input[0].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_MIDDLEDOWN;
                    break;

                default:
                    break;
            }

            SendInput(1, input, Marshal.SizeOf(input[0]));
        }

        private void mouseUp(int x, int y, mouseButton mouseButton)
        {
            //現在のカーソル位置と引数に指定されたカーソル位置が異なる場合のみ、マウスカーソルの移動を行う
            setMouseCursor(x, y);

            //指定されたボタンをクリックする     
            INPUT[] input = new INPUT[1];

            switch (mouseButton)
            {
                case mouseButton.left:
                    input[0].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_LEFTUP;
                    break;

                case mouseButton.right:
                    input[0].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_RIGHTUP;
                    break;

                case mouseButton.middle:
                    input[0].inputUnion.mouseInput.dwFlags = MouseAction.MOUSEEVENTF_MIDDLEUP;
                    break;

                default:
                    break;
            }

            SendInput(1, input, Marshal.SizeOf(input[0]));
        }

        //関数==========================================================================
        public void leftClick(int x = -1, int y = 1)//boolにしてエラー検知を行うかは要件等
        {
            mouseClick(x, y, mouseButton.left);
        }

        public void rightClick(int x = -1, int y = 1)//boolにしてエラー検知を行うかは要件等
        {
            mouseClick(x, y, mouseButton.right);
        }


        public void middleClick(int x = -1, int y = 1)//boolにしてエラー検知を行うかは要件等
        {
            mouseClick(x, y, mouseButton.middle);
        }

        //down
        public void leftDown(int x = -1, int y = 1)//boolにしてエラー検知を行うかは要件等
        {
            mouseDown(x, y, mouseButton.left);
        }

        public void rightDown(int x = -1, int y = 1)//boolにしてエラー検知を行うかは要件等
        {
            mouseDown(x, y, mouseButton.right);
        }


        public void middleDown(int x = -1, int y = 1)//boolにしてエラー検知を行うかは要件等
        {
            mouseDown(x, y, mouseButton.middle);
        }

        //up
        public void leftUp(int x = -1, int y = 1)//boolにしてエラー検知を行うかは要件等
        {
            mouseUp(x, y, mouseButton.left);
        }

        public void rightUp(int x = -1, int y = 1)//boolにしてエラー検知を行うかは要件等
        {
            mouseUp(x, y, mouseButton.right);
        }


        public void middleUp(int x = -1, int y = 1)//boolにしてエラー検知を行うかは要件等
        {
            mouseUp(x, y, mouseButton.middle);
        }

        public void typing(string key)
        {

            //ScanCodeShortは最低2文字なので、この方法で英数字キーを含め、すべてのScanCodeShortに対応するキーを一意に識別できる。
            if (key.Length == 1)
            {
                key = "KEY_" + key;
            }

            foreach (ScanCodeShort sc in Enum.GetValues(typeof(ScanCodeShort)))
            {
                if(key.Equals(sc.ToString()))
                {
                    //引数keyが示すスキャンコードをinput構造体に代入し、入力を実行する
                    INPUT[] input = new INPUT[1];
                    input[0].inputType = 1;
                    input[0].inputUnion.keyboardInput.wScan = sc;
                    input[0].inputUnion.keyboardInput.dwFlags = KEYEVENTF.SCANCODE;

                    SendInput(1, input, Marshal.SizeOf(input[0]));
                    break;
                }
            }
        }

        public void typingModifireKey(string key,string modifireKey)
        {
           
           //foreach (ScanCodeShort sc in Enum.GetValues(typeof(ScanCodeShort)))
            //{
                //引数keyが示すスキャンコードをinput構造体に代入し、入力を実行する
                /*
                INPUT[] input = new INPUT[4];
                input[0].inputType = 1;
                input[0].inputUnion.keyboardInput.wScan = ScanCodeShort.LSHIFT;
                input[0].inputUnion.keyboardInput.dwFlags = KEYEVENTF.SCANCODE;
                input[0].inputUnion.keyboardInput.time = 100;

                input[1].inputType = 1;
                input[1].inputUnion.keyboardInput.wScan = sc;
                input[1].inputUnion.keyboardInput.dwFlags = KEYEVENTF.SCANCODE;
                input[1].inputUnion.keyboardInput.time = 100;

                input[2].inputType = 1;
                input[2].inputUnion.keyboardInput.wScan = sc;
                input[2].inputUnion.keyboardInput.dwFlags = KEYEVENTF.KEYUP;
                input[2].inputUnion.keyboardInput.time = 100;
 
                input[3].inputType = 1;
                input[3].inputUnion.keyboardInput.wScan = ScanCodeShort.LSHIFT;
                input[3].inputUnion.keyboardInput.dwFlags = KEYEVENTF.KEYUP;
                input[3].inputUnion.keyboardInput.time = 100;
                SendInput(4, input, Marshal.SizeOf(input[0]));*/
               // input[2].inputUnion.keyboardInput.wVk = VirtualKeyShort.

                //////////////////////////////////
                INPUT[] input = new INPUT[2];
                input[0].inputType = 1;
                input[0].inputUnion.keyboardInput.wVk = VirtualKeyShort.VK_SHIFT;

                input[1].inputType = 1;
                input[1].inputUnion.keyboardInput.wVk = VirtualKeyShort.K_A;
                SendInput(2, input, Marshal.SizeOf(input[0]));

               // break;
            //}
        }


    }
}
