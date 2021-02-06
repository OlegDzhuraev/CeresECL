# CeresECL
Ceres ECL is **experimental** implementation of Entity Component Logic architectural pattern in Unity. 

<p align="center">
    <img src="http://dzhuraev.com/CeresECL/CeresECLUnity1.png" width="364" height="385" alt="Ceres ECL">
</p>

Ceres ECL is designed to be lightweight and used mostly from code, not from Unity Inspector. If you're looking for more Unity-connected thing, take look at [PlutoECL](https://github.com/InsaneOneHub/PlutoECL).

### What is Entity Component Logic?
It is inspired by Entity Component System and Components over Inheritance approaches. 
Difference from ECS is that there no Systems, which handles a lot of objects, but there is Logics, which handle self objects like MonoBehaviours in Unity. 
But with ECL you obliged to separate logics from data.
I really don't know is there exist any pattern like this, so I described it by my own. :)

Problem of Unity-way scripting is that there only MonoBehaviour, which is not saves you from bad coding practices. Ceres ECL will keep you in track of following the pattern principles.

### Why I should use it instead of Entity Component System?
No reasons. ECS is cool. But if you're have problems with understanding ECS or it looks too complicated to you,
Ceres ECL is simplier and looks more like usual Unity-way scripting, so you can use it.

## Overview
There is ready classes Entity, Component, Logic, which you should use to create your own. More info and examples will be added soon.

### Entity
**Entity** is **MonoBehaviour** script. It describes your object - contains all it **Components** and **Logics**. Looks like Unity component system, but all code is open-source.

```csharp
// Spawning new Entity object using PlayerEntityBuilder and using instance of playerPrefab as Entity GameObject.
var entity = Entity.Spawn<PlayerEntityBuilder>(playerPrefab);

// Spawn empty game object as entity (no prefab needed).
var emptyEntity = Entity.Spawn<PlayerEntityBuilder>();
```

### Component
Component is simple class, which only contains some data of your **Entity**. For example, **MoveComponent**, which contains **Speed** and **Direction** of movement. 
Should be no any logics code in this class.

```csharp
using CeresECL;

public class MoveComponent : Component
{
  public float Speed;
  public Vector3 Direction;
}
```

### Logic
**Logic** describes specific behaviour of the **Entity**. Logics should know nothing about anothers, it should work only with **Components**, **Events** and **Tags**.

For example, **MoveLogic** will move it using **MoveComponent** data. 
And **InputLogic** will fill **MoveComponent Direction** field with player input.


```csharp
using CeresECL;

public class MoveLogic : Logic, IInitLogic, IRunLogic
{
  MoveComponent moveComponent;

  void IInitLogic.Init()
  {
    moveComponent = Entity.Components.Get<MoveComponent>();

    moveComponent.Speed = 2f;
  }

  void IRunLogic.Run()
  {
    Entity.transform.position += moveComponent.Direction * (moveComponent.Speed * Time.deltaTime);
  }
}
```

There is **IInitLogic** interface to implement initialization logic, similar to the **Start** Unity method.

There also **IRunLogic** interface to implement run logic, similar to the **Update** Unity method.

### Builder
You need to create your entities, filling it with Logics which will handle this entity behaviour. **Builder** is Init Logic realization, designed to setup your entity Logics.
```csharp
using CeresECL;

public class PlayerEntityBuilder : Builder
{
  protected override void Build()
  {
    Entity.Logics.Add<InputLogic>();
    Entity.Logics.Add<MoveLogic>();
  }
}
```

Builders is a only one place, where you allowed to create strong dependencies (Builders will know about all connected Logics). This is key differnce from ECS -- in ECS most of dependencies is placed in Launcher, which generate ECS World with all it's system. So there 1 ECS Launcher file vs N ECL Builder files. But in other there should be minimum dependencies amount.

### Tags
If you need to create new component, which will be simple mark object state, use **Tags** instead. **Entity.Tags** contains all tags, added to the entity. 

**Tags** can be any **Enum**. You can use **TagsList.cs** from Example or create your own **enum Tag**.
To create new **Tag**, edit **enum Tag**:
```csharp
public enum Tag
{
    CustomTag = 0,
    // add your tags here
}
```

Adding tag to the entity:
```csharp
Entity.Tags.Add(Tag.CustomTag);
```

Note that tags are stacked, it means that you can add several same tags to the entity. It can used for some mechanics like block of some entity action from different Logics.

Check of tag on the entity:
```csharp
Entity.Tags.Have(Tag.CustomTag);
```

Removing tag (only one, if there stacked tags) from the entity:
```csharp
Entity.Tags.Remove(Tag.CustomTag);
```

New tags version is a simple integer in code, so if you want see your Tags names from enum in Entity debug, you need specify your enum type in **CeresSettings** in ECL Launcher script:
```csharp
CeresSettings.Instance.TagsEnum = typeof(Tag);
```
You can see it in **Example**.

Tags inspired by Pixeye Actors ECS tags. But possble that in my realisation this feature is not so cool. :D

### Events
**Events** - same to the components, but live only 1 frame. So if something happens in your game, you can send event from one of Logics and other will catch it. Since event same to the component, it can contain any data. To create **Event**, create new class, derived from **Event**:

```csharp
using CeresECL;

class ColliderHitEvent : Event
{
    public Collider2D HitCollider;
}
```

To send **Event**, do it like this:
```csharp
 var hitEvent = new ColliderHitEvent
{
    HitCollider = other
};

Entity.Events.Add(hitEvent);
```

You can send events not only from Logics, but from any MonoBehaviours too, it can be useful for combining of default Unity-way coding and ECL.
Note that **Logics** adding order can be important since **Event** live only one frame.

You can subscribe to event type in **Logic** like this:
```csharp
using CeresECL;

public class Bullet : Logic, IInitLogic
{
  void IInitLogic.Init()
  {
    Entity.Events.Subscribe<ColliderHitEvent>(OnHit);
  }

  void OnHit(ColliderHitEvent eventData)
  {
    // handle event data
  }
}
```

Unsubscribe is the same:
```csharp
Entity.Events.Unsubscribe<ColliderHitEvent>(OnHit);
```

Current Events version is not finished and can be unstable, but all tests passed fine, so I added it to the repo.

### Launcher
To make it all work, you need entry point, some classic **MonoBehaviour** script. To do this correct, create your new script, name it, for example, **MyLauncher**, and derive from Ceres **Launcher** class. Next, you need to override **StartAction** method and add there your init logic.

```csharp
using Ceres.ECL;

public class MyLauncher : Launcher
{
    protected override void StartAction()
    {
        Entity.Spawn<YourEntBuilder>();
    }
}
```

Base **Launcher** class handles all entities update, so there only 1 MonoBehaviour Update for **all** Entities Logics. For some unknown reasons, in Unity it increases game FPS. So do **not** make **Update** method in your Launcher, it can override Ceres one. And, yes, you don't need it in any case.

### Templates
You can create each of these classes using templates. Click **RMB** in **Project Window** and open submenu **Ceres ECL**. There will be all actual templates. 

It will generate script in root namespace, which you can change in **Editor Settings -> Editor -> C# Project Generation**. Otherwise it will be generated in **CeresECL** namespace.

For template generator idea thanks to LeoECS, some used things came from its code.

### Dependency Injection
Ceres ECL has DI implementation for Logics. So you can inject any data to all of yours Entity Logics:

```csharp
Entity.Logics.Inject(data);
```

Your Logic should have field with same to **data** type, for example, if **data** type is **Data**, your Logic should look like this:

```csharp
using CeresECL;

public class TestLogic : Logic, IRunLogic
{
  Data injectedData;

  void IRunLogic.Run()
  {
    // Do smth ?
  }
}
```

Dependency Injection idea came from LeoECS, you can find a link to its repo at the end of readme.

## Debug
To check state of your **Entity**, select its GameObject on scene and you will see all **Tags**, **Components** and **Logics**, added to the entity with their parameters:
<p align="center">
    <img src="http://dzhuraev.com/CeresECL/CeresECLUnity1.png" width="364" height="385" alt="Ceres ECL">
</p>

## More FAQ
### Can I edit variables or entities from Inspector
**No**. All changes should be done from code - this is place for all your game logic. If you need add some data - load it from **Scritable Object** files and use Dependency Injection. If you prefer using Inspector for all your settings, take look at [PlutoECL](https://github.com/InsaneOneHub/PlutoECL), it is much more Inspector-friendly.

### How I can use OnTriggerEnter, for example
You can create classic **MonoBehaviour**, and send Event from it to the Entity.Events. Check Example, there is **CollisionSender** class. In [PlutoECL](https://github.com/InsaneOneHub/PlutoECL) you can use all these methods directly in Logic code. And no, this is not Ad for PlutoECL :D

### Is Ceres ECL production-ready
No, until there will be at least one release on GitHub. Currently it is fully experimental non-commercial project. But you can use it on your risk, all features already should work.

## Examples
Check Example folder from repository, it contains simple Ceres ECL usage example. 

Links to games examples on GitHub will be added when these examples will be created. :)

## Thanks to
Leopotam for LeoECS https://github.com/Leopotam/ecs

Pixeye for Actors https://github.com/PixeyeHQ/actors.unity

Inspired me to use ECS and think moer about different coding architecture patterns. :)
