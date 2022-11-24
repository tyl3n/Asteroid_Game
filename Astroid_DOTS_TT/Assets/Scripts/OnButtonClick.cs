using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class OnButtonClick : MonoBehaviour
{
    public Button m_button;
    
	void Start () {
        
		Button btn = m_button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
        SceneManager.LoadScene(0);
	}
}
