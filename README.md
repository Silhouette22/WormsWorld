# WormsWorld

Данный проект был создан в ходе прохождения курса по .NET/C# в НГУ. 

Ниже представлены поставленные задачи для текущей ветки. 

## Задача 1. Мир червячков, ч1 (основы).

Червячки живут в плоском двумерном бесконечно мире целочисленном мире [int x, int y]. Червячок - это точка в этом мире. 

Жизнь длится 100 ходов.

### У червячка есть следующие атрибуты:

* Имя.

* Позиция в двумерном мире.

### Взаимодействие мира и червячка:

1. Симулятор запрашивает у i-го червячка, что тот собирается делать.
2. Червячок может ответить (червячок в момент принятия решения имеет информацию обо всем мире, т.е. ему известно положение еды, и всех червячков на карте, номер текущего хода также известен):
    *   Сдвинуться (направо, налево, вверх, вниз).
    *   Ничего.
3. Если ничего, то все остается на своем месте (ход истрачен).
4. Если целевая клетка занята (другим червячком), то все остается на своем месте (ход истрачен).
5. Если ход допустим, то червячок сдвигается в нужном направлении.

Изначально в мир заселяется червяк в точке (0, 0), необходимо реализовать движение червячка по часовой стрелке вокруг точки (0,0).

В результате должно получиться консольное приложение .Net Core, после запуска которого, должен сгенерироваться файл следующего формата:

 
*Worms: [John (-1, 0)]*

*Worms: [John (-2, 0)]*

*…*

*Worms: [John (-10, 0)]*

Где * john - имя червячка


## Задача 2. Мир червячков, ч2 (еда, размножение).

У червячка добавляется параметр жизненная сила. Каждый ход жизненная сила червячка уменьшается на 1, червячок с жизненной силой 0 удаляется из мира, “умирает”.

В начала хода, перед ходом всех червячков на свободном месте появляется порция еды.

### Свободное место определяется следующем образом:

* Каждая координата (x,y) генерируется вводом случайного значения, нормально распределенного на множестве целых чисел [int]*.

* Вероятность выпадения значения i распределена нормально со среднеквадратичным отклонением 5 и матожиданием 0.

* Если точка (x,y) занята другой порцией еды, то происходит новая генерация случайной точки, до тех пор пока не будет найдено свободное место.

* Если точка (x,y) занята червячком, то червячок "съедает” ее в начале своего хода (потом может сдвинуться на другую клетку и съесть еще порцию).

Еда существует в мире ровно 10 ходов (включая тот ход, на котором она появляется), на 11-ход еда удаляется из мира, "протухает”. Еда увеличивает жизненную силу червячка на 10.

### Червячок, может размножится делением, для этого:

1. указывается направление (верх, вниз, вправо, влево)

2. если точка по указанному направлению свободна, то в ней появляется новый червячок с новым уникальным именем.


### Взаимодействие мира и червячка:    	

1. Симулятор запрашивает у i-го червячка, что тот собирается делать.

2. Червячок может ответить (для принятия решения он также имеет всю информацию):

    * Сдвинуться (вправо, влево, вверх, вниз).

    * Размножиться, указав направление (вправо, влево, вверх, вниз).

    * Ничего (ход истрачен).

3. Если ничего, то все остается на своем месте.

4. Если червячок хочет сдвинуться:

    * Если целевая точка занята (другим червяком), то все остается на своем месте.

    * Если целевая точка свободна, то червячок перемещается на свободное место.

    * Если целевая точка занята порцией еды, то червячок “съедает ее”, жизненная сила червячка увеличивается на 10, еда удаляется из мира. 

5. Если червячок хочет размножиться:

    * Если жизненная сила червячка 10 или меньше, то ничего не происходит (ход истрачен).

    * Если целевая клетка занята другим червячком, то ничего не происходит (ход истрачен).

    * Если целевая клетка занята едой, то ничего не происходит (ход истрачен).

    * Если целевая клетка свободна, на ней появляется червячок с жизненной силой 10 и уникальным именем, жизненная сила исходного червячка уменьшается на 10.

** В любом случае, в конце хода, жизненная сила червячка уменьшается на 1.

Изначально в мир заселяется червяк в точке (0, 0), реализовать движение червячка к ближайшей порции еды.

В результате должно получиться консольное приложение .Net Core, после запуска которого, должен сгенерироваться файл следующего формата:

*Worms: [John-23 (-1, 0)], Food: [(1,1), (1,1), (1,1)]*

*Worms: [John-22 (-2, 0)], Food: [(1,1), (1,1), (1,1)]*

*…*

*Worms: [John-1 (-10, 0)], Food: [(1,1), (1,1), (1,1)]*

Где * john - имя червячка, 22, 23 жизненная сила.
