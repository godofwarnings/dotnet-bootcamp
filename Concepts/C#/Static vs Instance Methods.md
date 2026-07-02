# Static vs Instance Methods

In C#:
- A **static method** belongs to the class itself, not to an object instance. 
- An **instance method** belongs to an object, so it can only be called through an instantiated object.

## The `Main` Method
`Main()` is static. It belongs to the `Program` class itself, meaning the runtime does not need to create an object of `Program` to start the application.

## Calling Methods
If you try to call a non-static method from inside a static method (like `Main`), you will get a compiler error. This is because the non-static method requires an object instance to exist, but the static context has no instance.

To fix this, you must either:
1. Make the target method `static` (so it also belongs to the class).
2. Instantiate an object of the class inside `Main` and call the method on that object.
