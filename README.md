# battery-devices-master

## Setup Instruction 

1. Install Node.js. <br/>
   Installation instructions can be found here: https://nodejs.org/en/download
2. Install nswag — automatic code generation tool. <br/>
  `npm install nswag@14.0.7 -g`
3. Restore NuGet packages. <br/>
  `dotnet restore`
4. Build project. <br/>
  `make` (Unix) <br/> OR <br/> `nswag run apigen.nswag` and `nswag run clientgen.nswag` (Windows)
5. Run application. <br/>
  `./run` (Unix) <br/> OR <br/> `dotnet run --project server -lp https`

## Итерация 2

[Скринкаст работы приложения](https://drive.google.com/file/d/1P27lMn_53urH26l79JT_YdW_Vn4TxeT_/view?usp=sharing)

[Презентация](https://docs.google.com/presentation/d/1HEqx0hy5rIVZiJH80dui4qmq9hYMK1HpA-GMuQUQEFE/edit?usp=sharing)