#include "gl_helper.hpp"

//
#include "geometry.hpp"

int main() {
    int programID = initGLProgram("Water");

    if (programID == -1) {
        fprintf(stderr, "something went wrong while initializing");
        return programID;
    }

    //BoxGeometry *box = new BoxGeometry(1.0f, 1.0f, 1.0f);
    Geometry *imagePlane = new Geometry();
    imagePlane->vertices = {
        -1.0f, 1.0f, 0.0f,
        -1.0f, -1.0f, 0.0f,
        1.0f, -1.0f, 0.0f,
        1.0f, 1.0f, 0.0f};

    imagePlane->indices = {
        0, 1, 3,
        3, 1, 2};
    //TrackballControl *trackBallControl = new TrackballControl(window, camera);
    // TODO: implement scene graphs
    //render(camera, box);
    render(imagePlane);
}