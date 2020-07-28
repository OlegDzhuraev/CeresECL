# CeresECL
Ceres ECL is implementation of Entity Component Logic architectural pattern in Unity. 

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
Entity is MonoBehaviour script. It describes your object - contains all it Components and Logics. Looks like Unity component system, but all code is open-source.

```csharp
// Spawning new Entity object using PlayerEntityBuilder and using instance of playerPrefab as Entity GameObject and filling it with new logic (for example, it should be done in builder).
var entity = Entity.Spawn<PlayerEntityBuilder>(playerPrefab);
entity.AddLogic<MoveLogic>();
```

### Component
Component is simple class, which only contains some data of your Entity. For example, MoveComponent, which contains Speed and Direction of movement. 
Should be no any logics code in this class.

```csharp
public class MoveComponent : Component
{
  public float Speed;
  public Vector3 Direction;
}
```

### Logic
Logic describes specific behaviour of the Entity. For example, MoveLogic will move it using MoveComponent data. 
And InputLogic will fill MoveComponent Direction field with player input.

```csharp
public class MoveLogic : Logic, IInitLogic, IRunLogic
{
  MoveComponent moveComponent;

  void IInitLogic.Init()
  {
    moveComponent = Entity.Get<MoveComponent>();

    moveComponent.Speed = 2f;
  }

  void IRunLogic.Run()
  {
    Entity.transform.position += moveComponent.Direction * (moveComponent.Speed * Time.deltaTime);
  }
}
```

### Builder
You need to create your entities, filling it with Logics which will handle this entity behaviour. Builder is Init Logic realization, designed to setup your entity Logics.
```csharp
public class PlayerEntityBuilder : Builder
{
  protected override void Build()
  {
    Entity.AddLogic<InputLogic>();
    Entity.AddLogic<MoveLogic>();
  }
}
```

## Examples
Check Example folder from repository, it contains simple Ceres ECL usage example. 

Links to games examples on GitHub will be added when these examples will be created. :)

## Thanks to
Leopotam for LeoECS https://github.com/Leopotam/ecs

Pixeye for Actors https://github.com/PixeyeHQ/actors.unity

Inspired me to use ECS and think moer about different coding architecture patterns. :)
