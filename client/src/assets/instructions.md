# Настройки файлов

Настройки представляют из себя конфигурационные файлы, оформленных в виде .yaml файлов.

Существует два вида конфигурационных файлов:
- схема параметров
- конструктор окон

В данной инструкции будут приведены примеры кода, для настройки конфигурационных файлов:
***
```yaml
пример строки кода
# после решётки идёт строка с коментарием, не влияющая на код
ещё одна строка кода
```
***

# Схема параметров

Схема параметров состоит из самих параметров и шаблонов, которые можно использовать для объявления параметров:
- `templates`;
- `parameters`.

## Параметр

**Параметр** — настраиваемое значение, которое будет записано в базу данных устройства.

Обязательные поля параметра:
- `name: string` — уникальное наименование, может быть произвольным;
- `type: enum` — тип параметра

Дополнительные общие поля:
- `enabled: boolean` — включён ли параметр;
- `nullable: boolean` — может ли параметр принимать нулевое значение (по умолчанию `false`);
- `description: string` — описание параметра, отображающееся при наведении на `?`;
- `hint: string` — подсказка, отображающаяся в пустом поле ввода параметра;
- `default: any` — значение параметра по умолчанию;
- `conditions: array` — условия настроек параметра;
- `using` — использование некоторого шаблона (дальше в инструкции);
- `for` — использование цикла (дальше в инструкции).

### Типы параметров

Параметры могут быть атомарными: простыми, сложными — и составными.

К простым параметрам относятся те, что задаются текстовым вводом указанного формата:
- `integer` — целое число;
- `decimal` — число с плавающей запятой повышенной точности;
- `string` — произвольная строка;
- `date` — дата (гггг/мм/дд);
- `time` — время (чч/мм).

Сложные параметры:
- `selector` — одно из вручную указанных значений, каждое из которых может иметь описание. По умолчанию значениями селектора являются возрастающие от нуля числа, однако их можно вручную изменить;
***
```yaml
  name: version
  type: selector
  items:
    - first
    - second
```
***
  ИЛИ
***
```yaml
  name: version
  type: selector
  items:
    - label: first
      value: 01
      description: Lorem ipsum dolor sit amet
    - label: second
      value: 10
      description: Ut enim ad minim veniam
  ```
***
- `array` — динамический список пользовательских значений определённого типа;
***
  ```yaml
  name: signals
  type: array
  items:
      type: integer
      min: 0
  ```
***

Составной параметр — параметр, объединяющий несколько атомарных параметров в рамках одного. Составной параметр является *надпараметром*, группирующий примитивные параметры в один единый параметр.
***
```yaml
name: totalVolume
type: composite
allOf:
  - volumeA
  - volumeB
  - measure
```
***
> **ВНИМАНИЕ**
> Составной параметр не может содержать в себе другой составной параметр!

### Фильтры (ограничения)

В зависимости от типа параметра к нему могут быть применены некоторые ограничивающие фильтры:
- `min: [integer, decimal]` — ограничение числа снизу;
- `max: [integer, decimal]` — ограничение числа сверху;
- `precision: integer` — ограничение количества цифр после запятой;
- `regex: string` — соответствие строки регулярному выражению;

По фильтру автоматически генерируются подсказки:
- `0...10000` для `min: 0` и `max: 10000`
- `0.00` для `precision: 2`
- `^[-+]?\d+$` для `regex: ^[-+]?\d+$`, где ^[-+]?\d+$ — регулярное выражение

> **ВАЖНО**
> Текст, указанный в настройке `hint`, имеет приоритет в отображении выше, чем автоматические подсказки по фильтру.

### Условия параметров

Существует несколько условий:
- `dependsOn: string` — включённость параметра с некоторым наименованием;
- `oneOf: array` — равенство указанного в `name` параметра одному из значений.

> **ВАЖНО**
> Приоритеты условий уменьшаются сверху вниз.

В зависимости от условия могут изменяться следующие поля:
- `description`;
- `enabled`;
- фильтры.

### Цикл

Для упрощения создания некоторого количества одинаковых параметров используются **циклы**.

Цикл настраивается через поле параметра `for` в форме `START:END:STEP`, причём $\mathrm{START} \le i \le \mathrm{END}$. Переменной `i` можно пользоваться внутри других полей параметра, заключая символ в фигурные скобки (`{i}`) с возможностью применения простых арифметических операций (сложение, вычитание, умножение и деление и т. п.).

> В примере используется шаблон `temperature`.
***
```yaml
name: t_{i}
using: temperature:{i}:{10 - i * 10}:{10 + 10 % i}
for: 1:6:1
```
***

## Шаблон

**Шаблон** — это «заготовка», которая может использоваться при создании параметров с подстановкой некоторых данных.

В качестве полей шаблона используются те же настройки, что и для обычного параметра. Тело шаблона будет целиком (за исключением наименования) скопировано в тело параметра.

Кроме того, шаблон можно параметризовать данными, которые будут использованы в конкретном месте, — места, которые необходимо «заполнить» обозначаются через `$N`, где `N` — номер аргумента (начиная с 1).

Для использования шаблона при объявлении параметра используется поле `using` с именем шаблона и передачей необходимых аргументов через символ двоеточия (`:`).
***
```yaml
templates:
  # объявление шаблона
  - name: temperature
    description: Температура тела в точке $1.
    type: decimal
    precision: 2
    min: $2
    max: $3

parameters:
  # использование шаблона
  - name: t_1
    using: temperature:1:0:99.99
```
***

> **ВАЖНО**
> Если случается коллизия полей шаблона и параметра, то приоритет отдаётся полям параметра.

# Конструктор окон

Конфигурационный файл отвечает за то, какие параметры будут отображены в приложении, а также их группировка по вкладкам.

Обязательные поля:
- `name: string` — наименование конструктора;
- `tabs: array` — список вкладок.

## Вкладки

Весь процесс создания базы данных разбит на этапы, каждый из которых представлен отдельной **вкладкой**. Каждая вкладка ответственна за настройку некоторых параметров.

Обязательные поля вкладки:
- `name: string` — наименование вкладки;
- `description: string` — описание вкладки;
- `parameters: array` — список наименований параметров, которые будут настраиваться в рамках этой вкладки.

> **ВАЖНО**
> Нумерация вкладок идёт сверху вниз.