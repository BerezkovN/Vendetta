using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] public CameraEffects _cameraEffects;
    [SerializeField] public GameObject _enemiesObject;
    [SerializeField] public DialogueBox _dialogueBox;

    private Interactable? _currentTarget = null;
    public PlayerInfo _playerInfo;
    public Movement _movement;

    private bool _pressed = false;
    private List<Enemy> enemies;

    public bool Dead { get => _playerInfo.Health <= 0; }
    public bool Alive { get => _playerInfo.Health > 0; }
    private bool EveryoneDead { 
        get  {
            bool return_value = true;
            foreach (Enemy enemy in enemies)
            {
                if (!enemy.Dead) {
                    return_value = false;
                    break;
                }
            }
            return return_value;
        }
    }
    private void Start()
    {
        _playerInfo = GetComponent<PlayerInfo>();
        _movement = GetComponent<Movement>();
        if (_cameraEffects == null)
        {
            throw new System.ArgumentNullException("_cameraEffects cannot be null!");
        }
        enemies = new List<Enemy>();
        foreach(Transform transform_ in _enemiesObject.transform)
        {
            foreach (Transform transform in transform_)
            {
                if (transform.TryGetComponent(out Enemy enemy))
                {
                    enemies.Add(enemy);
                }
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            _dialogueBox.AddTextToQueue(
                "Автор: Вендета? Що ж...",
                1, 1
                );
            _dialogueBox.AddTextToQueue(
               "Автор: Мабуть найвідоміша історія помсти. Кровної помсти за страждання народу.",
               4, 1
               );
            _dialogueBox.AddTextToQueue(
               "Автор: З давніх часів історія революцій в Англії була актуальна.",
               3, 1
               );
            _dialogueBox.AddTextToQueue(
               "1190 рік від народження Христа.\nТериторія Графства Гемпшир\n68 км від Лондона. Поселення південних кельтів",
               6, 1
               );
            _dialogueBox.AddTextToQueue(
               "Автор: Помста за сім'ю, аргумент...",
               2, 1
               );
            _dialogueBox.AddTextToQueue(
               "Вільям: Ці виродки заплатять за смерть моєї сім'ї.",
               2, 1
               );
            _dialogueBox.AddTextToQueue(
               "Вільям: У них блокпост з башнею на сході. Вони люблять вогонь... Завітаю до них.",
               3, 1
               );
        } 
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _dialogueBox.AddTextToQueue(
                "Лондон\n1605 рік",
                1, 1
                );
            _dialogueBox.AddTextToQueue(
               "Автор: Ех, Лондон. Чарівне місто. Шкода, що податок великий і голодний народ...",
               4, 1
               );
            _dialogueBox.AddTextToQueue(
               "Ґай Фокс: Там під трибуною сюрприз для парламентера. Сірники на місці, можна рушати...",
               5, 1
               );
            _dialogueBox.AddTextToQueue(
               "Солдати: Ей ти! Фокс! Зловити його!",
               3, 1
               );
            _dialogueBox.AddTextToQueue(
               "Ґай Фокс: Ви всі пошкодуєте! Революція в душі у кожного з вільних людей! Ваш режим паде!",
               4, 1
               );
            _dialogueBox.AddTextToQueue(
               "Автор: Що ж, трішки у нього не вийшло. А знатний міг бути вибух...",
               3, 1
               );
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {

            _dialogueBox.AddTextToQueue(
               "Лондон\n2006",
               4, 1
               );
            _dialogueBox.AddTextToQueue(
               "Автор: Тут напевно я помовчу..",
               2, 1
               );
            _dialogueBox.AddTextToQueue(
               "Автор: Помста. Яке цікаве поняття. Починається завжди з горя, а закінчується пустотою...",
               4, 1
               );
        }
    }
    float _escPressedEnd = 0;
    bool flag = false;
    float _everyoneDeadEnd = 0;
    void Update()
    {
        if(EveryoneDead)
        {
            _cameraEffects.SetExitFade(1 - (_everyoneDeadEnd - Time.realtimeSinceStartup) / 3);

            if (!flag)
            {
                _everyoneDeadEnd = Time.realtimeSinceStartup + 3;
                flag = true;
            }
            // todo scene loading
            if (_everyoneDeadEnd < Time.realtimeSinceStartup) {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    SceneManager.LoadScene(1);
                    flag = false;
                }
                else
                {
                    SceneManager.LoadScene(2);
                    _everyoneDeadEnd = Time.realtimeSinceStartup + 3;
                    flag = false;
                }
            }
            return;
        }
        if(_playerInfo.Health <= 0)
        {
            if (!flag)
            {
                flag = true;
                _escPressedEnd = Time.realtimeSinceStartup + 5;
            }
            _cameraEffects.SetExitFade(1 - (_escPressedEnd - Time.realtimeSinceStartup) / 5);
            if (_escPressedEnd < Time.realtimeSinceStartup)
            {
                Application.Quit();
            }
            return;
        }
        if (_currentTarget != null)
        {
            if (Input.GetButtonDown(_currentTarget.Key.KeyName))
            {
                _pressed = true;
                _currentTarget.OnClick(_playerInfo);
            }
            else if (_pressed && !Input.GetButton(_currentTarget.Key.KeyName))
            { 
                _pressed = false;
                _currentTarget.OnClickExit(_playerInfo);
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            _escPressedEnd = Time.realtimeSinceStartup + 3;
        } 
        else if(Input.GetButton("Cancel"))
        {
            _cameraEffects.SetExitFade(1 - (_escPressedEnd - Time.realtimeSinceStartup) / 3);
            if(_escPressedEnd < Time.realtimeSinceStartup)
            {
                Application.Quit();
            }
        } 
        else
        {
            _escPressedEnd = 0;
            _cameraEffects.SetExitFade(0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Interactable>(out Interactable obj) && 
            !obj.IsHidden())
        {
            if (_currentTarget != null)
            {
                _pressed = false;
                _currentTarget.TriggerExit(collision);
            }
            _currentTarget = obj;
            _currentTarget.TriggerEnter(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Interactable>(out Interactable obj) && 
            _currentTarget != null && obj == _currentTarget)
        {
            _pressed = false;
            _currentTarget.TriggerExit(collision);
            _currentTarget = null;
        }
    }

    public void TakeDamage(int damage)
    {
        _playerInfo.ApplyDamage(damage);
        _cameraEffects.OnTakingDamage();
        if(_playerInfo.Health <= 0)
        {
            _movement.CallState(Movement.States.Die);
        }
        if (SceneManager.GetActiveScene().buildIndex == 0 && _dialogueBox)
        {
            _dialogueBox.AddTextToQueue(
               "Вільям: Згинь, кельтський покидьок!",
               2, 1
               );
        }
    }
    public void StealthAttack()
    {
        _cameraEffects.OnSilentKill();
        _movement.CallState(Movement.States.StealthAttack);
    }
    public void Attack()
    {
        _cameraEffects.OnHit();
        _movement.CallState(Movement.States.Attack);

    }
}
