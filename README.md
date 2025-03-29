# OverCooked


## Summary

This game is a study case for the game Overcooked! 2.

All art assets like Textures, models, audio clips etc where created by the indie game maker **Code Monkey** for this [Free Complete Unity Course](https://youtu.be/AmGSEH7QcDg). 

However, the game structure and the source-code were created by me and they are very different from the course.

This project uses many [Unity Packages](http://34.125.146.81:4873/) created and published by me. All of them are on the MIT license so you can freely use them into your project.


## Controls

- **Tab** - Switch between Chefs.
- **AWSD** or **Arrow Keys** - Movement.
- **E** - Interact with Items (Cutting Table, Stove Table) 
- **F** - Interact with Plate and Ingredients (Tomato, Cheese, Bread etc)

> Gamepad is also supported.

## How To add new Recipes

1. Inside the [Recipe folder](/Assets/Prefabs), create a new Recipe Data asset by using the Create menu, Kitchen Chaos > Recipes > Recipe;
2. Open the [IngredientsToRecipe prefab](/Assets/Prefabs/Player.prefab) and link the new Recipe asset into the Recipe field;

    ![image](https://github.com/user-attachments/assets/f1982c11-201a-4251-837d-44b9d48335e9)

3. Place each child ingredient in the right position;
