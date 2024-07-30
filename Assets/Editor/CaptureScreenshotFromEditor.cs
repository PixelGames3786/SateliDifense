using UnityEditor;
using UnityEngine;

namespace djkusuha.Utility
{
    /// <summary>
    /// Unity�G�f�B�^�ォ��Game�r���[�̃X�N���[���V���b�g���B��Editor�g��
    /// </summary>
    public class CaptureScreenshotFromEditor : Editor
    {
        /// <summary>
        /// �L���v�`�����B��
        /// </summary>
        /// <remarks>
        /// Edit > CaptureScreenshot �ɒǉ��B
        /// HotKey�� Ctrl + Shift + F12�B
        /// </remarks>
        [MenuItem("Tools/CaptureScreenshot #%F12")]
        private static void CaptureScreenshot()
        {
            // ���ݎ�������t�@�C����������
            var filename = "C:/Users/TomozawaKaikou/Documents/UnityScreenshot/"+System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".png";
            // �L���v�`�����B��
#if UNITY_2017_1_OR_NEWER
            ScreenCapture.CaptureScreenshot(filename); // �� GameView�Ƀt�H�[�J�X���Ȃ��ꍇ�A���̎��_�ł͎B���Ȃ�
#else
            Application.CaptureScreenshot(filename); // �� GameView�Ƀt�H�[�J�X���Ȃ��ꍇ�A���̎��_�ł͎B���Ȃ�
#endif
            // GameView���擾���Ă���
            var assembly = typeof(EditorWindow).Assembly;
            var type = assembly.GetType("UnityEditor.GameView");
            var gameview = EditorWindow.GetWindow(type);
            // GameView���ĕ`��
            gameview.Repaint();

            Debug.Log("ScreenShot: " + filename);
        }
    }
}