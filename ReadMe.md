# Reactive Programming 101 (RP101)
It's sometimes difficult to convince business owners, project managers and other developers to use the reactive 
programming (RP) approach to software development. There are of course trade-offs to be made, although the benefits 
of reactive programming often outweigh the costs.

Perhaps many developers reject reactive programming because they're so used to object-oriented programming (OOP), 
and they'd rather stick to what they know best. If you don't know how to build a reactive program, or the benefits 
to be claimed by doing so, then choosing the OOP approach to software development makes perfect sense.

But don't be fooled into thinking that using reactive programming means that you won't be using OOP. To the contrary, 
you may find that you're still using more OOP than RP even when you take an RP-first approach. In fact, OOP assumes
a significant supporting role in any modern application that is built with RP at its core.

In this video series, instead of attempting to define "Reactive Programming", or throwing around buzz words, or presenting 
slides that list the benefits and omit the downsides, we're going to use the scientific approach.

## We're Going To Build an Application!
Using a reactive-first approach, I'll design and implement a client-server chat program from the ground up.

[![RP101](https://img.youtube.com/vi/WBn4ZhR7C94/0.jpg)](https://www.youtube.com/playlist?list=PLzLa5EktSmly4OP2nvzGfwdbVDn52d1rM)

All of my coding sessions will be recorded in a screen cast video series, accompanied by verbal explanations as I'm coding. None 
of the design or the work has been planned ahead of time, except for the first draft of the
[specification](https://github.com/RxDave/ReactiveProgramming/blob/master/Spec.md). None of my coding will be 
edited<sup>1</sup>, so all of my poor design decisions, to my shame, will be recorded for all to see.

The benefit of recording bad design decisions, and recording myself noticing them, and hopefully fixing them, including refactoring 
and debugging, is that I won't be sugar coding the experience for anyone asking the question, "Is reactive programming right for me?"
Undoubtedly, you must decide this for yourself, but much of the literature and videos out there right now don't provide a 
comprehensive view, including the costs vs. benefits. I'm hoping to change that with this video series.

The entire program will be written in C#, although, I'll try to make my explanations useful in general for the many implementations
of Rx on other platforms, such as Java and JavaScript.

The source code will be available for free in [my GitHub repo](https://github.com/RxDave/ReactiveProgramming), updated with each 
new video in the series as each video is published.

I may also try to leave some questions unanswered at the end of each video, so that viewers can try to solve problems themselves
based on what they've learned in the video. The code up to that point will have been checked in, so viewers can checkout the latest
code and continue coding from where I left off. Then, at the beginning of the subsequent video, I'll show my code and explain how I 
decided to solve those problems myself. This is something that I'd really like to do, but without any planning upfront it may be 
tricky, so I'm not going to commit to it just yet.

<small>
<sup>1</sup>Except perhaps long stretches of dullness or annoyances will be edited out; e.g., when I have to read documentation to 
understand a third-party API before I can use it to add a feature, or if an audio segment is full of "umms" and "uhhs".
Suggestions and constructive criticisms are welcomed.
</small>
<p></p>

##### Some of the topics to be covered are: 
* An introduction to Rx (Reactive Extensions).
* The proper use of Subjects.
* A brief look at observable temperature (hot and cold).
* Managing side effects
* Debugging
* Performance analysis and tuning
* Testing

##### Required Prerequisite Knowledge
It is highly recommended that you are comfortable with all of the items in the following list, because I won't be 
providing any in-depth explanations about them in the videos.
* **.NET Framework** or a comparable library; e.g., Java.
* **C#** or a comparable C-style, GC'd language; e.g., Java, JavaScript.
* **Visual Studio** or a comparable IDE.
* **OOP** (Object-Oriented Programming)

##### Recommended Prerequisite Knowledge
The following is not required knowledge, but recommended if you want a smoother learning experience focused on RP alone.
Being comfortable with these concepts will mean less confusion for you around the high-level RP design concepts, although 
I'll provide some basic explanations anyway as each topic becomes relevant.

* **Functional Programming** - just the basics; e.g., higher-order functions, tuples, purity.
* **LINQ** (Language INtegrated Query)\
  I'll certainly provide some explanation for the non-C# developers out there, but it won't be in-depth. There are many 
  resources out there on the web to teach you the depths of LINQ, and anyway much of the queries will be using the
  fluent-method call syntax that most non-C# developers are familiar with already. If you don't know much about LINQ, 
  then for our purpose of learning RP, I'll just focus my explanation on the basic usage and leave many assumptions for 
  you to research on your own.

# Getting Started

1. Read the [features document](https://github.com/RxDave/ReactiveProgramming/blob/master/Features.md) to get yourself
   acquainted with the concepts of the chat application that we'll be building.
2. Glance over the [technical specification](https://github.com/RxDave/ReactiveProgramming/blob/master/Spec.md) if you
   want to look ahead at what the video series covers in more detail. In many of the videos, perhaps the most important
   regarding RP in general, I'll be coding directly against the specification.
3. Prepare your environment if you wish to code along. Here's the environment that I'll be using in the video series:
   1. Windows 10
   1. Visual Studio Community 2017
   1. .NET Framework 4.6.2
