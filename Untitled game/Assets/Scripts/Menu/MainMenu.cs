using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField]
    Animator animator;
    public GameObject buttonPrefab;
    AsyncOperation async;

void Start()
    {
        Canvas canvas_ui = GetComponent<Canvas>();
        animator = GameObject.Find("Fader").GetComponentInChildren<Animator>();


        //Actions
        Action start = () =>
        {
            PlayerPrefs.SetInt("LevelCleared", 0);
            animator.SetTrigger("LoadGame");
        };
        Action load = () =>
        {
            animator.SetTrigger("LoadGame");
        };
        Action quit = () =>
        {
            Application.Quit();
        };
        Action setResolutionHD = () =>
        {
            Screen.SetResolution(1920, 1080, false);
        };
        Action setResolution = () =>
        {
            Screen.SetResolution(1440, 900, false);
        };
        Action setResolutionbad = () =>
        {
            Screen.SetResolution(500, 500, false);
        };
        Action setlocForest = () =>
        {
            StaticTestSettings.setLocation(Locations.Forest);
        };
        Action setlocDesert = () =>
        {
            StaticTestSettings.setLocation(Locations.Desert);
        };
        Action setlocVillage = () =>
        {
            StaticTestSettings.setLocation(Locations.Village);
        };
        Action setmapSmall = () =>
        {
            StaticTestSettings.setMapSize(new Vector2Int(50, 50));
        };
        Action setmapMed = () =>
        {
            StaticTestSettings.setMapSize(new Vector2Int(100, 100));
        };
        Action setmapLarge = () =>
        {
            StaticTestSettings.setMapSize(new Vector2Int(200, 200));
        };
        Action setTypeDungeon = () =>
        {
            StaticTestSettings.SetMapType(MapType.Dungeon);
        };
        Action setTypeVillage = () =>
        {
            StaticTestSettings.SetMapType(MapType.Village);
        };
        Action setTypeRuins = () =>
        {
            StaticTestSettings.SetMapType(MapType.Ruins);
        };

        AMC_Factory f = new AMC_Factory(buttonPrefab, canvas_ui);
        MenuElement r1 = new MenuElement("HD", setResolutionHD);
        MenuElement r2 = new MenuElement("Okay", setResolution);
        MenuElement r3 = new MenuElement("Bad", setResolutionbad);
        


        MenuElement newGame = new MenuElement("New Game", start);
        MenuElement loadGame = new MenuElement("Load", load);
        MenuElement quitGame = new MenuElement("Quit", quit);

        AbstractMenuComposite settings = f.createSubmenu("Settings", new List<AbstractMenuComposite>
        {
            r1,
            r2,
            r3
        });

        List<AbstractMenuComposite> list = new List<AbstractMenuComposite>
        {
            newGame,
            loadGame,
            settings,
            quitGame
        };
        AbstractMenuComposite backToMenu = f.createSubmenu("Back", list);
        settings.AddComponent(backToMenu);

        AbstractMenuComposite play = f.createSubmenu("Play", list);


        MenuElement SetLocForest = new MenuElement("Forest",setlocForest);
        MenuElement SetLocDesert = new MenuElement("Desert",setlocDesert);
        MenuElement SetLocVillage = new MenuElement("Village",setlocVillage);

        MenuElement setMapSizeSmall = new MenuElement("50x50", setmapSmall);
        MenuElement setMapSizeMedium = new MenuElement("100x100", setmapMed);
        MenuElement setMapSizeLarge = new MenuElement("200x200", setmapLarge);

        MenuElement setTypeDung = new MenuElement("Dungeon", setTypeDungeon);
        MenuElement setTypeRuin = new MenuElement("Ruins", setTypeRuins);
        MenuElement setTypeVill = new MenuElement("Village", setTypeVillage);

        AbstractMenuComposite testMapTypes = f.createSubmenu(StaticTestSettings.GetMapType().ToString(), new List<AbstractMenuComposite>
        {
            setTypeDung,
            setTypeRuin,
            setTypeVill,
        });
        Action updateTypeName = () =>
        {
            testMapTypes.ButtonName = StaticTestSettings.GetMapType().ToString();
        };

        AbstractMenuComposite testLocations = f.createSubmenu(StaticTestSettings.getLocation().ToString(), new List<AbstractMenuComposite>
        {
            SetLocForest,
            SetLocDesert,
            SetLocVillage,
        });
        Action updateLocName = () =>
        {
            testLocations.ButtonName = StaticTestSettings.getLocation().ToString();
        };

        
        AbstractMenuComposite testMapSize = f.createSubmenu(StaticTestSettings.getMapSize().ToString(), new List<AbstractMenuComposite>
        {
            setMapSizeSmall,
            setMapSizeMedium,
            setMapSizeLarge,
        });

        Action updateSizeName = () =>
        {
            testMapSize.ButtonName = StaticTestSettings.getMapSize().ToString();
        };


        MenuElement testStart = new MenuElement("Test start", start);


        List<AbstractMenuComposite> testList = new List<AbstractMenuComposite>
        {
            testLocations,
            testMapSize,
            testMapTypes,
            testStart
        };
        AbstractMenuComposite test = f.createSubmenu("Test", testList);

        Action back = () =>
        {
            test.ClickOperation();
        };
        setTypeDung.AppendFunction(updateTypeName);
        setTypeRuin.AppendFunction(updateTypeName);
        setTypeVill.AppendFunction(updateTypeName);
        setTypeDung.AppendFunction(back);
        setTypeRuin.AppendFunction(back);
        setTypeVill.AppendFunction(back);

        SetLocForest.AppendFunction(updateLocName);
        SetLocDesert.AppendFunction(updateLocName);
        SetLocVillage.AppendFunction(updateLocName);
        SetLocForest.AppendFunction(back);
        SetLocDesert.AppendFunction(back);
        SetLocVillage.AppendFunction(back);

        setMapSizeSmall.AppendFunction(updateSizeName);
        setMapSizeMedium.AppendFunction(updateSizeName);
        setMapSizeLarge.AppendFunction(updateSizeName);
        setMapSizeSmall.AppendFunction(back);
        setMapSizeMedium.AppendFunction(back);
        setMapSizeLarge.AppendFunction(back);

        List<AbstractMenuComposite> l2 = new List<AbstractMenuComposite> { play , test };
        AbstractMenuComposite menuTop = f.createSubmenu("Start Menu",l2);
        play.AddComponent(menuTop);
        test.AddComponent(menuTop);

        menuTop.ClickOperation();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("LoadGame") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
        {
            if (async == null)
            {
                async = SceneManager.LoadSceneAsync("LoadingScene");
                async.allowSceneActivation = false;
                animator.SetTrigger("LoadingGame");
            }   
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("black") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
        {
            async.allowSceneActivation = true;
        }

    }
}

public class AMC_Factory
{
    GameObject button;
    Canvas canvas;

    public AMC_Factory(GameObject button, Canvas canvas)
    {
        this.button = button;
        this.canvas = canvas;
    }


    public AbstractMenuComposite createSubmenu(string name,List<AbstractMenuComposite> listOfButtons)
    {
        AbstractMenuComposite submenu = new SubMenu(name,button,canvas);
        if(listOfButtons != null && listOfButtons.Count != 0)
        {
            foreach (AbstractMenuComposite btn in listOfButtons)
            {
                submenu.AddComponent(btn);
            }
        }
        return submenu;
    }

}

public abstract class AbstractMenuComposite
{
    public string ButtonName;
    protected GameObject buttonPrefab;
    protected Canvas canvas_ui;

    protected AbstractMenuComposite(string buttonName, GameObject buttonPrefab, Canvas canvas_ui)
    {
        ButtonName = buttonName;
        this.buttonPrefab = buttonPrefab;
        this.canvas_ui = canvas_ui;
    }

    public abstract void ClickOperation();

    public virtual void AddComponent(AbstractMenuComposite amc)
    {
        throw new System.NotImplementedException();
    }

    public virtual void RemoveComponent(AbstractMenuComposite amc)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool IsComposite()
    {
        return false;
    }

    protected void CreateButton(AbstractMenuComposite amc)
    {
        GameObject b1 = UnityEngine.Object.Instantiate(buttonPrefab);

    }
}

public class SubMenu : AbstractMenuComposite
{
    private List<AbstractMenuComposite> abstractMenus = new List<AbstractMenuComposite>();

    public override bool IsComposite()
    {
        return true;
    }

    public SubMenu(string buttonName, GameObject buttonPrefab, Canvas canvas_ui) : base(buttonName, buttonPrefab, canvas_ui)
    {
    }

    public override void AddComponent(AbstractMenuComposite amc)
    {
        abstractMenus.Add(amc);
    }

    public override void RemoveComponent(AbstractMenuComposite amc)
    {
        abstractMenus.Remove(amc);
    }

    public override void ClickOperation()
    {
        CreateButtons(abstractMenus);
    }

    private void CreateButtons(List<AbstractMenuComposite> abstractMenus)
    {
        int size = abstractMenus.Count;
        foreach (Button item in canvas_ui.GetComponentsInChildren<Button>())
        {
            Debug.Log(item.name);
            UnityEngine.Object.Destroy(item.gameObject);
        } 
        float v_offset = (canvas_ui.pixelRect.height - size * 60)/2;
        for (int i = 0; i < size; i++)
        {   
            GameObject currentButton = UnityEngine.Object.Instantiate(buttonPrefab);
            currentButton.transform.SetParent(canvas_ui.transform);
            currentButton.transform.position = new Vector3(canvas_ui.pixelRect.width / 2, canvas_ui.pixelRect.height - ( v_offset + 60* i));
            currentButton.GetComponentInChildren<Text>().text = abstractMenus[i].ButtonName;
            if (abstractMenus[i].IsComposite())
            {
                SubMenu subMenu = ((SubMenu)abstractMenus[i]);
                currentButton.GetComponent<Button>().onClick.AddListener(() => subMenu.ClickOperation());
            }
            else
            {
                MenuElement menuItem = ((MenuElement)abstractMenus[i]);
                currentButton.GetComponent<Button>().onClick.AddListener(() => menuItem.ClickOperation());
            }
        }
       
    }
}

public class MenuElement : AbstractMenuComposite
{
    Action function;

    public MenuElement(string title, Action action) : base(title, null, null)
    {
        function = action;
    }

    public override void ClickOperation()
    {
        function.Invoke();
    }

    public void AppendFunction(Action action)
    {
        function += action;
    }

}