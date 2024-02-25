using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VNGameController : MonoBehaviour
{
    public GameScene currentScene;
    public BottomBarController bottomBar;
    public SpriteSwitcher backgroundController;
    public OptionsLogicController optionsController;

    private State state = State.IDLE;
    private bool isLastScene = false;

    private string targetScene; // Name of the scene to load

    private enum State
    {
        IDLE, ANIMATE, CHOOSE
    }

    void Start()
    {
        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;
            bottomBar.PlayScene(storyScene);
            backgroundController.SetImage(storyScene.background);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (state == State.IDLE && bottomBar.IsCompleted())
            {
                if (bottomBar.IsLastSentence())
                {
                    StoryScene storyScene = currentScene as StoryScene;
                    StoryScene cs = storyScene;
                    isLastScene = cs.isLast();
                    Debug.Log(isLastScene);
                    if (isLastScene == false)
                    {
                        PlayScene((currentScene as StoryScene).nextScene);
                    }
                    else
                    {
                        targetScene = cs.tScene();
                        LoadTargetScene();
                    }
                }

                else
                {
                    bottomBar.PlayNextSentence();
                }
            }
            
        }
    }

    public void PlayScene(GameScene scene)
    {
        StartCoroutine(SwitchScene(scene));
    }

    private IEnumerator SwitchScene(GameScene scene)
    {
        state = State.ANIMATE;
        currentScene = scene;
        bottomBar.Hide();
        yield return new WaitForSeconds(1f);
        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;
            backgroundController.SwitchImage(storyScene.background);
            yield return new WaitForSeconds(1f);
            bottomBar.ClearText();
            bottomBar.Show();
            yield return new WaitForSeconds(1f);
            bottomBar.PlayScene(storyScene);
            state = State.IDLE;
        }
        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            optionsController.SetupChoose(scene as ChooseScene);
        }
    }
    private void LoadTargetScene()
    {
        SceneManager.LoadScene(targetScene); // Load the target scene
    }
}
