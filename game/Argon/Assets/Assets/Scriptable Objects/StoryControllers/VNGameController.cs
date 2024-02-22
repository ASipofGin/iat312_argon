using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Rendering;

public class VNGameController : MonoBehaviour
{
    // Start is called before the first frame update

    public StoryScene currScene;
    public BottomBarController bottomBar;
    public BackgroundController backgroundController;
    void Start()
    {
        bottomBar.PlayScene(currScene);
        backgroundController.SetImage(currScene.background);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (bottomBar.IsCompleted())
            {
                if (bottomBar.IsLastSentence())
                {
                    currScene = currScene.nextScene;
                    bottomBar.PlayScene(currScene);
                    backgroundController.SwitchImage(currScene.background);
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
                
            }
        }
    }
}
