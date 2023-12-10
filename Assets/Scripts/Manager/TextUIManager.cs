using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class TextUIManager : MonoBehaviour
    {
        public static TextUIManager instance;
        private float _showTime = 0f;
        private bool _beginShow = false;
        private GameObject _currentUI = null;

        private void Awake()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (_beginShow)
            {
                _showTime -= Time.deltaTime;
            }
            else
            {
                if (_currentUI)
                {
                    _currentUI.SetActive(false);
                    _currentUI = null;
                }
            }

            if (_showTime <= 0)
            {
                _beginShow = false;
            }
        }

        public void ShowUI(GameObject ui, float time = 0.1f)
        {
            if (!ui) return;
            if (_currentUI)
            {
                _currentUI.SetActive(false);
                _currentUI = null;
            }
            _currentUI = ui;
            _beginShow = true;
            _showTime = time;
            _currentUI.SetActive(true);
        }
    }

}