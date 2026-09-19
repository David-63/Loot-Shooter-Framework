using Dave6.LootShooter.Networking.Bootstrap;
using Dave6.LootShooter.Networking.Session;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dave6.LootShooter.Samples.Networking.UI
{
    public class NetworkDebugPanel : MonoBehaviour
    {
        NetworkBootstrap _Bootstrap;
        NetworkSessionController _Session;

        Button _HostButton;
        TextField _JoinCodeField;
        Button _ClientButton;
        Button _ShutdownButton;
        Label _StatusLabel;

        void OnEnable()
        {
            _Bootstrap = GetComponent<NetworkBootstrap>();
            _Bootstrap.OnReady += BindSession;
            if (_Bootstrap.IsReady) BindSession();
        }
        void OnDisable()
        {
            _Bootstrap.OnReady -= BindSession;
            var panel = GetComponent<PanelRenderer>();
            panel.UnregisterUIReloadCallback(InitialUI);

            if (_Session == null) return;
            ClearUI();
            _Session.OnStateChanged -= UpdateUI;
            _Session.OnJoinCodeChanged -= UpdateJoinCode;
        }

        public void BindSession()
        {
            _Session = _Bootstrap.Session;
            var panel = GetComponent<PanelRenderer>();
            panel.RegisterUIReloadCallback(InitialUI);

            _Session.OnStateChanged += UpdateUI;
            _Session.OnJoinCodeChanged += UpdateJoinCode;
        }
        void InitialUI(PanelRenderer renderer, VisualElement root)
        {
            _HostButton = CreateButton("HostButton", "Host");

            _JoinCodeField = new TextField("Join Code") { name = "JoinCodeField" };
            _JoinCodeField.style.width = 240;

            _ClientButton = CreateButton("ClientButton", "Client");
            _ShutdownButton = CreateButton("ShutdownButton", "Shutdown");
            _StatusLabel = CreateLabel("StatusLabel", "Not Connected");

            _HostButton.clicked += HandleHostButtonCliked;
            _ClientButton.clicked += HandleClientButtonCliked;
            _ShutdownButton.clicked += HandleShutdownButtonCliked;

            root.Add(_HostButton);
            root.Add(_JoinCodeField);
            root.Add(_ClientButton);
            root.Add(_ShutdownButton);
            root.Add(_StatusLabel);
        }
        void ClearUI()
        {
            _HostButton.clicked -= HandleHostButtonCliked;
            _ClientButton.clicked -= HandleClientButtonCliked;
            _ShutdownButton.clicked -= HandleShutdownButtonCliked;
        }

        async void HandleHostButtonCliked()
        {
            await _Session.CreateSessionAsync();
        }
        async void HandleClientButtonCliked()
        {
            string code = _JoinCodeField.value.Trim();
            if (string.IsNullOrEmpty(code))
            {
                _StatusLabel.text = "Join code is empty.";
                return;
            }

            // if (string.Equals(_Session.JoinCode, code))
            // {
            //     _StatusLabel.text = "Join code is incorrect.";
            //     return;
            // }

            await _Session.JoinSessionAsync(code);
        }
        async void HandleShutdownButtonCliked()
        {
            await _Session.LeaveSessionAsync();
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
                _JoinCodeField.SetEnabled(true);
                return;
            }

            if (!_Session.IsClient && !_Session.IsServer)
            {
                SetStartButtons(true);
                _JoinCodeField.SetEnabled(true);
            }
            else
            {
                SetStartButtons(false);
                _JoinCodeField.SetEnabled(!_Session.IsServer);
                UpdateStatusLabels();
            }
        }
        void UpdateJoinCode(string code)
        {
            _JoinCodeField.value = $"{code}";
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
