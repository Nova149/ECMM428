# ECMM428
4th year Research Project, with the goal of comparing different timetabling algorithms for a university, where the algorithms must assign asynchronous, online and in-person lectures.

The goal of a timetabling algorithm is to assign all required lectures to certain rooms at certain times, whilst ensuring that the timetable is as high quality as possible. 
The quality of a timetable is subjective, however we can define what we find important in a timetable in the form of constraints. There are two types of constraints: 
Hard constraints, which are required to be fulfilled, and soft constraints, which should be fulfilled if possible are not necessary. 
For example, a hard constraint might be that a lecturer should not be assigned two lectures at the same time, while a soft constraint might be that a student should not have exactly one lecture on a certain day. 
We apply penalties when constraints are not met, with hard constraints applying far larger penalties than soft constraints. 
The sum of these penalties is called the fitness; this is the numerical measure of quality of the timetable, where a higher fitness means a lower quality. So this is the number that we want to minimize with our algorithm.

In this project I have primarily used nature-inspired algorithms. These use two operators: 
The first is the mutation operator, which in this context means swapping a lecture from a particular timeslot with a different random timeslot, and also moving it to a different venue if needed. 
The second is the crossover operator, which means taking two lectures from two different timetables and swapping their timeslots.

I have used two types of nature-inspired algorithms. Both involve keeping two timetables at a time, which are acted on by the operators.
The first is the genetic algorithm, which repeatedly applies one of the operators, and only keeps the result if it is better.
The second is the differential evolution algorithm. This algorithm applies mutation and then crossover operators on both parent timetables. If one or both child timetables are better, they replace the oldest parent or parents.

Many teaching institutions are adopting the practice of hybrid learning. This had already started before the COVID-19 pandemic, and has increased in usage since. The aim of this project is to modify existing timetabling algorithms to also schedule algorithms that are not in-person.

For the purposes of this project, we define an online lecture as one set at a particular time, but with no venue, and an asynchronous lecture as having no set time and no venue.
Having these remote lectures are useful since they are less likely to contribute significant fitness to the timetable as a whole.
