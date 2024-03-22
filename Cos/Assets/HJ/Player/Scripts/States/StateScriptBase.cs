using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

// StateMachineBehaviour
// https://docs.unity3d.com/kr/530/ScriptReference/StateMachineBehaviour.html
// StateMachineBehaviour는 state machine state에 추가할 수 있는 component입니다. 이는 state의 모든 스크립트가 파생되는 base class입니다.
// 기본적으로 Animator는 controller에 정의된 각 behaviour의 새 인스턴스를 인스턴스화합니다. 클래스 속성 SharedBetweenAnimatorsAttribute는 behaviours이 인스턴스화되는 방식을 제어합니다.
// StateMachineBehaviour에는 OnStateEnter, OnStateExit, OnStateIK, OnStateMove, OnStateUpdate와 같은 사전 정의된 메시지가 있습니다.

// SharedBetweenAnimatorsAttribute
// https://docs.unity3d.com/kr/530/ScriptReference/SharedBetweenAnimatorsAttribute.html
// SharedBetweenAnimatorsAttribute는 이 StateMachineBehaviour가 한 번만 인스턴스화되고 모든 Animator 인스턴스 간에 공유되어야 함을 명시하는 속성입니다. 이 속성은 각 컨트롤러 인스턴스의 메모리 공간을 줄입니다.
// It's up to the programmer to choose which StateMachineBehaviour could use this attribute. Be aware that if your StateMachineBehaviour change some member variable it will affect all other Animator instance using it. See Also: StateMachineBehaviour class.
// 이 속성을 사용할 수 있는 StateMachineBehaviour를 선택하는 것은 프로그래머의 몫입니다. StateMachineBehaviour가 일부 멤버 변수를 변경하면 해당 변수를 사용하는 다른 모든 Animator 인스턴스에 영향을 미친다는 점을 명심하여야 합니다.

// ===== public 함수 =====

// OnStateMachineEnter
// StateMachine으로 transition이 생길 때 첫 번째 Update 프레임에서 호출됩니다. StateMachine sub-state로 transition 생길 때는 호출되지 않습니다.
// 실제로 이 콜백을 받으려면 평가 흐름이 Entry 노드를 통과해야 합니다.

// OnStateMachineExit
// StateMachine에서 transition할 때 마지막 업데이트 프레임에서 호출됩니다. StateMachine sub-state로 전환할 때는 호출되지 않습니다.
// 실제로 이 콜백을 얻으려면 평가 흐름이 Exit 노드를 통과해야 합니다.

// ===== 메시지 =====

// StateMachineBehaviour.OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
// transition이 시작되고 state machine이 이 상태를 평가할 때 첫번째 Update 프레임에서 호출됩니다.

// StateMachineBehaviour.OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
// 첫 번째 프레임과 마지막 프레임을 제외한 각 Update 프레임에서 호출됩니다.

// StateMachineBehaviour.OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
// transition이 종료되고 state machine이 이 상태 평가를 마칠 때 마지막 Update 프레임에서 호출됩니다.

// StateMachineBehaviour.OnStateMove(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
// MonoBehaviour.OnAnimatorMove 바로 뒤에 호출됩니다.

// StateMachineBehaviour.OnStateIK(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
// MonoBehaviour.OnAnimatorIK 바로 뒤에 호출됩니다.


// ===== 상속된 멤버 =====

// === 변수 ===

// Object.hideFlags
// 객체를 숨겨야 하는지, Scene과 함께 저장되거나 또는 User에 의해 수정될 수 있는지?
// https://docs.unity3d.com/kr/530/ScriptReference/Object-hideFlags.html


// Object.name
// 객체의 이름. Component는 Game object 및 연결된 모든 component와 동일한 이름을 공유합니다.
// https://docs.unity3d.com/kr/530/ScriptReference/Object-name.html

// === public 함수 ===

// Object.GetInstanceID
// object의 인스턴스 ID는 항상 고유함이 보장됩니다.
// https://docs.unity3d.com/kr/530/ScriptReference/Object.GetInstanceID.html

// Object.ToString
// Game object의 Object.name을 반환합니다.
// https://docs.unity3d.com/kr/530/ScriptReference/Object.ToString.html

// === static 함수 ===

// Object.Destroy
// public static void Destroy (Object obj, float t= 0.0F);
// Game object, Component, 또는 asset을 제거합니다.
// Object obj는 지금 파괴되거나 지금으로부터 t초 후 지정된 시간에 파괴됩니다. object의 실제 파괴는 항상 언제나 업데이트 루프 이후까지 지연되지만 렌더링 전에 완료됩니다.
// obj가 Component인 경우 GameObject에서 해당 컴포넌트를 제거하고 파괴합니다.
// obj가 GameObject인 경우 해당 GameObject, 모든 Component 및 모든 변형 자식이 삭제됩니다.
// https://docs.unity3d.com/kr/530/ScriptReference/Object.Destroy.html

// Object.DestroyImmediate
// public static void DestroyImmediate (Object obj, bool allowDestroyingAssets = false);
// Object obj를 즉시 파괴됩니다. Object.Destroy를 대신 사용할 것이 강력하게 권장됩니다.
// 이 함수는 edit 모드에서는 지연된 파괴가 호출되지 않으므로 editor 코드를 작성할 때에만 사용되어야 합니다. game 코드에서는 대신 Object.Destroy를 사용하여야 합니다.
// 파괴는 언제나 지연됩니다(그러나 동일한 프레임 내에서 실행됩니다). 이 기능을 사용하는 것은 asset을 영구적으로 파괴할 수 있기 때문에 주의하여 사용하여야 합니다!
// 또한 배열을 반복하고 반복중인 요소를 파괴해서는 안되는 것을 알아야 합니다. 이것은 심각한 문제를 야기할 수 있습니다.(Unity 뿐만이 아닌 일반적인 프로그래밍 관행에서도)

// Object.DontDestroyOnLoad
// public static void DontDestroyOnLoad (Object target);
// Makes the object target not be destroyed automatically when loading a new scene.
// When loading a new level all objects in the scene are destroyed, then the objects in the new level are loaded.In order to preserve an object during level loading call DontDestroyOnLoad on it. If the object is a component or game object then its entire transform hierarchy will not be destroyed either.

// Object.FindObjectOfType
// public static Object FindObjectOfType (Type type);
// 반환 : Object An array of objects which matched the specified type, cast as Object.
// Returns the first active loaded object of Type type.
// Please note that this function is very slow. It is not recommended to use this function every frame. In most cases you can use the singleton pattern instead.
// See Also: Object.FindObjectsOfType.

// Object.FindObjectsOfType
// public static Object[] FindObjectsOfType (Type type);
// 반환 : Object[] The array of objects found matching the type specified.
// Returns a list of all active loaded objects of Type type.
// It will return no assets (meshes, textures, prefabs, ...) or inactive objects.
// Please note that this function is very slow. It is not recommended to use this function every frame. In most cases you can use the singleton pattern instead.

// Object.Instantiate
// public static Object Instantiate (Object original);
// public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
// 반환 : Object A clone of the original object.
// https://docs.unity3d.com/kr/530/ScriptReference/Object.Instantiate.html

// ScriptableObject.CreateInstance
// public static ScriptableObject CreateInstance (string className);
// public static ScriptableObject CreateInstance(Type type);
// public static T CreateInstance();
// 반환 : ScriptableObject The created ScriptableObject.
// Creates an instance of a scriptable object.
// To easily create a ScriptableObject instance that is bound to a .asset file via the Editor user interface, consider using CreateAssetMenuAttribute.

// === 연산자 ===

// Object.bool

// Object.operator !=

// Object.operator ==

// === 메시지 ===

// ScriptableObject.Awake()
// This function is called when the ScriptableObject script is started.
// Awake is called as the ScriptableObject script starts. This happens as the game is launched and is similar to MonoBehavior.Awake).
// An example is given below. This example has two scripts. The first shown is the ScriptableObject script. This implements code which is separate from MonoBehaviour. The second is a small MonoBehaviour related script which accesses values from the ScriptableObject script.

// ScriptableObject.OnDestroy()
// This function is called when the scriptable object will be destroyed.
// OnDestroy cannot be a co-routine.

// ScriptableObject.OnDisable()
// This function is called when the scriptable object goes out of scope.
// This is also called when the object is destroyed and can be used for any cleanup code. When scripts are reloaded after compilation has finished, OnDisable will be called, followed by an OnEnable after the script has been loaded.
// OnEnable cannot be a co-routine.

// ScriptableObject.OnEnable()
// This function is called when the object is loaded.
// OnEnable cannot be a co-routine.

namespace HJ
{
    public class StateScriptBase : StateMachineBehaviour
    {
        // OnStateEnter : transition이 시작되고 state machine이 이 상태를 평가할 때 첫번째 Update 프레임에서 호출됩니다.
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        // OnStateUpdate : OnStateEnter와 OnStateExit 콜백 사이의 각 Update 프레임에서 호출됩니다
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        // OnStateExit : transition이 종료되고 state machine이 이 상태 평가를 마칠 때 마지막 Update 프레임에서 호출됩니다.
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        // OnStateMove : Animator.OnAnimatorMove() 바로 뒤에 호출됩니다.
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 루트 모션을 처리하고 영향을 미치는 코드 구현
        }

        // OnStateIK: Animator.OnAnimatorIK() 바로 뒤에 호출됩니다.
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 애니메이션 IK(inverse kinematics)를 설정하는 코드 구현
        }
    }
}