using UnityEngine;

namespace Assets.Plugins.Helpers
{
    public static class MouseOrTouchInput
    {
        /// <summary>
        /// Возвращает true, если была нажата левая кнопка мыши, либо произошло касание
        /// </summary>
        /// <returns></returns>
        public static bool IsFirstDown()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#elif UNITY_IPHONE || UNITY_ANDROID
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#else
            return false;
#endif
        }

        /// <summary>
        /// Возвращает true, если была нажата левая кнопка мыши, либо произошло касание. Срабатывает, даже если предыдущее первое касание не было отменено
        /// </summary>
        /// <returns></returns>
        public static bool IsFirstDownAny()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#elif UNITY_IPHONE || UNITY_ANDROID
            // TODO Проверить работоспособность Any() и вернуть первый вариант в случае ошибки
            //return Input.touchCount > 0 && Input.touches.Where(a => a.phase == TouchPhase.Began).ToList().Count > 0;
            return Input.touches.Any(a => a.phase == TouchPhase.Began);
#else
            return false;
#endif
        }

        /// <summary>
        /// Возвращает true, если левая кнопка мыши была отпущена либо касание отменено
        /// </summary>
        /// <returns></returns>
        public static bool IsUp()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return Input.GetMouseButtonUp(0);
#elif UNITY_IPHONE || UNITY_ANDROID
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
#else
            return false;
#endif
        }

        /// <summary>
        /// Возвращает true, если левая кнопка мыши была отпущена либо любое из совершенных ранее касаний было отменено
        /// </summary>
        /// <returns></returns>
        public static bool IsUpAny()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return Input.GetMouseButtonUp(0);
#elif UNITY_IPHONE || UNITY_ANDROID
            return Input.touchCount > 0 && Input.touches.Where(a => a.phase == TouchPhase.Ended).ToList().Count > 0;
#else
            return false;
#endif
        }

        /// <summary>
        /// Возвращает true, если ни левая кнопка мыши не нажата либо касания отсутствуют
        /// </summary>
        /// <returns></returns>
        public static bool IsNoTouches()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return !Input.GetMouseButton(0);
#elif UNITY_IPHONE || UNITY_ANDROID
            return Input.touchCount == 0; 
#else
            return false;
#endif
        }

        /// <summary>
        /// Возрващает true,если левая кнопка мыши нажата и удерживается либо присутствуют касания
        /// </summary>
        /// <returns></returns>
        public static bool IsDown()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return Input.GetMouseButton(0);
#elif UNITY_IPHONE || UNITY_ANDROID
            return Input.touchCount > 0;
#else
            return false;
#endif
        }

        /// <summary>
        /// Возвращает положение курсора мыши либо первого касания. Возвращает пустой вектор, если касания отсутствуют
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetCursorPosition()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return Input.mousePosition;
#elif UNITY_IPHONE || UNITY_ANDROID
            return Input.touchCount > 0
                ? new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y)
                : Vector3.zero;
#else
            return Vector2.zero;
#endif
        }
    }
}