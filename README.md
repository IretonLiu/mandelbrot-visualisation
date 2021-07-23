# simple-mandelbrot-visualiser
A simple visualisation of the mandelbrot set implemented in OpenGL. The calculations are done in GLSL, therefore in the GPU, which accelerates the process compared to a implementation on the CPU.

In the visualisation of the mandelbrot set, the x and y position are taken as the real and imaginary component of c, thus making the visualisation possible.

<figure>
 <img
 src = "./screenshots/screenshot_1.png "
 >
 <figcaption> Overview of the mandelbrot set</figcaption>
</figure>

<figure>
 <img
 src = "./screenshots/screenshot_2.png "
 >
 <figcaption> Zoomed in at coordinate (-0.748348181, 0.1)</figcaption>
</figure>

Some other ways of coloring the mandelbrot set:

<figure>
 <img
 src = "./screenshots/screenshot_3.png "
 >
 <figcaption> Colored using the orbit trap algorithm.
</figure>


<figure>
 <img
 src = "./screenshots/screenshot_4.png "
 >
 <figcaption> Colored using the arctan of the components.
</figure>

For more information on the Mandelbrot set, checkout these useful links:
* https://en.wikipedia.org/wiki/Mandelbrot_set
* https://en.wikibooks.org/wiki/Fractals/Iterations_in_the_complex_plane/orbit_trap

