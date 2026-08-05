using Dave6.LootShooter.Networking.Bootstrap;
using Dave6.LootShooter.Networking.Session;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dave6.LootShooter.Samples.Networking.UI
{
    public class NetworkDebugPanel : MonoBehaviour
    {
        [SerializeField] NetworkBootstrap _Bootstrap;
        NetworkSessionController _Session;

        Button _HostButton;
        Button _ClientButton;
        Button _ShutdownButton;
        Label _StatusLabel;

        void OnEnable()
        {
            _Bootstrap.OnReady += BindSession;
        }
        void OnDisable()
        {
            _Bootstrap.OnReady -= BindSession;
            var panel = GetComponent<PanelRenderer>();
            panel.UnregisterUIReloadCallback(InitialUI);

            if (_Session == null) return;
            ClearUI();
            _Session.OnStateChanged -= UpdateUI;
        }

        public void BindSession()
        {
            _Session = _Bootstrap.Session;
            var panel = GetComponent<PanelRenderer>();
            panel.RegisterUIReloadCallback(InitialUI);

            _Session.OnStateChanged += UpdateUI;
        }
        void InitialUI(PanelRenderer renderer, VisualElement root)
        {
            _HostButton = CreateButton("HostButton", "Host");
            _ClientButton = CreateButton("ClientButton", "Client");
            _ShutdownButton = CreateButton("ShutdownButton", "Shutdown");
            _StatusLabel = CreateLabel("StatusLabel", "Not Connected");

            _HostButton.clicked += _Session.StartHost;
            _ClientButton.clicked += _Session.StartClient;
            _ShutdownButton.clicked += _Session.Shutdown;

            root.Add(_HostButton);
            root.Add(_ClientButton);
            root.Add(_ShutdownButton);
            root.Add(_StatusLabel);
        }
        void ClearUI()
        {
            _HostButton.clicked -= _Session.StartHost;
            _ClientButton.clicked -= _Session.StartClient;
            _ShutdownButton.clicked -= _Session.Shutdown;
        }
        Button CreateButton(string name, string text)
        {
            var button = new Button { name = name, text = text };
            button.style.width = 240;
            button.style.backgroundColor = Color.white;
            button.style.color = Color.black;
            button.style.unityFontStyleAndWeight = FontStyle.Bold;
            return button;
        }
        Label CreateLabel(string name, string content)
        {
            var label = new Label { name = name, text = content };
            label.style.color = Color.black;
            label.style.fontSize = 18;
            return label;
        }

        void UpdateUI(NetworkSessionState state)
        {
            _StatusLabel.text = state.ToString();

            if (_Session.IsNetworkAvailable() == false)
            {
                SetStartButtons(false);
                return;
            }

            if (!_Session.IsClient && !_Session.IsServer)
            {
                SetStartButtons(true);
            }
            else
            {
                SetStartButtons(false);
                UpdateStatusLabels();
            }
        }


        void SetStartButtons(bool enabled)
        {
            _HostButton.SetEnabled(enabled);
            _ClientButton.SetEnabled(enabled);
            _ShutdownButton.SetEnabled(!enabled);
        }
        void UpdateStatusLabels()
        {
            string transport = "Transport: " + _Session.GetTransportName();
            string mode = "Mode: " + _Session.GetNetworkStatus();
            _StatusLabel.text = $"{transport}\n{mode}";
        }
    }
}
