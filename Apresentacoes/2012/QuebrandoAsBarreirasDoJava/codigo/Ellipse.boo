import system

class Ellipse (Figure)
   protected w
   protected h

   def construct (x,y,rw,rh)
       super(x,y)
       w = rw
       h = rh

   override def print ():
       Console.Write("Ellipse [w:" + w + " h:" + h "] ")
       super()
