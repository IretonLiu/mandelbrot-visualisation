#version 400 core

#define MAX_STEPS 100
#define SURFACE_DIST 0.01
#define MAX_DIST 100

precision highp float;

// Interpolated values from the vertex shaders
// in vec3 fragmentColor;
uniform ivec2 u_resolution;
uniform float u_time;

out vec3 color;

struct Camera {
  vec3 ro; // ray origin
  vec3 rd; // ray direction
};

// returns the smooth minimum of 2 values with smooth factor k
float smoothMin(float a, float b, float k) {
  float h = clamp(0.5 + 0.5 * (b - a) / k, 0., 1.);
  return mix(b, a, h) - k * h * (1.0 - h);
}

mat2 rotate(float angle) {
  float sinA = sin(angle);
  float cosA = cos(angle);
  return mat2(cosA, sinA, -sinA, cosA);
}

float sceneSDF(vec3 p) {

  // float dSphere = sphereSDF(p, vec4(1, 1, -5, 1));

  // vec3 spherePos = vec3(3, 1.5, -7);
  // spherePos.y += sin(u_time * 2);

  // float dSphere2 = sphereSDF(p, vec4(spherePos, .5));
  // vec3 boxPos = p - vec3(-2, 1, -5);
  // boxPos.xz = rotate(u_time) * boxPos.xz;
  // float dBox = boxSDF(boxPos, vec3(1));

  // float dPlane = p.y;
  // float dCapsule = capsuleSDF(p, vec3(1.5, 3, -4), vec3(1.5, 1, -4), .5);
  // float dTorus = torusSDF(p - vec3(0, 1, -5), vec2(1.2, 0.2));

  // // taking max of two values will give the intersection
  // // this is because the value will only be 0(small enough)
  // // at the place where the 2 object intersect
  // float dScene = min(
  //     min(smoothMin(max(-dCapsule, dSphere), smoothMin(dTorus, dBox, .2),
  //     .2),
  //         dSphere2),
  //     dPlane);
  return 1.0;
}

float rayMarch(vec3 ro, vec3 rd) {
  float dOrigin = 0.0; // distance from origin
  for (int i = 0; i < MAX_STEPS; i++) {
    vec3 p = ro + dOrigin * rd; // march the point forward in the direction

    // get distance of  closest object in the scene to the point
    // float dScene = sceneSDF(p); // distance to scene
    float dScene = sceneSDF(p);
    dOrigin += dScene;

    // we have a hit or if ray marches too far
    if (dScene < SURFACE_DIST || dOrigin > MAX_DIST)
      break;
  }
  return dOrigin;
}

vec3 getNormal(vec3 p) {

  vec2 e = vec2(0.01, 0);
  // intuition taking the gradient of the point and points around it
  // this will give a vector perpendicular, which is the normal;
  vec3 N = vec3(sceneSDF(p + e.xyy) - sceneSDF(p - e.xyy),
                sceneSDF(p + e.yxy) - sceneSDF(p - e.yxy),
                sceneSDF(p + e.yyx) - sceneSDF(p - e.yyx));

  return normalize(N);
}

vec3 lightingCalculation(vec3 p) {
  vec3 lightPos = vec3(0, 5, -3);
  vec3 L = normalize(lightPos - p);
  vec3 N = getNormal(p);

  float diffColor = clamp(dot(N, L), 0.0, 1.0);

  float d = rayMarch(p + N * SURFACE_DIST * 2,
                     L); // ray march in the direction of the light
  if (d < length(lightPos - p)) {
    diffColor *= 0.1;
  }

  return vec3(diffColor);
}

// coloring the mandelbrot set using the escape time algorithm
vec3 mandelbrotEscapeTime(vec2 uv) {
  float iter = 0;
  const float max_iter = 400;
  // real and imaginery component of a complex number
  vec2 z = vec2(0.);

  for (int i = 0; i < max_iter; i++) {

    z = vec2(z.x * z.x - z.y * z.y, 2.0 * z.x * z.y) + uv;

    if (length(z) > 3.)
      break;
    iter++;
  }

  // color based on the angle of the complex number;
  float angle = atan(z.y / z.x);
  float f = angle / 3.1415926;

  // color based on the escape time
  // float f = iter/max_iter;

  // coloring
  if (f < 0.33) {
    return vec3(f, 0., 0.);
  } else if (f >= 0.33 && f <= 0.67) {
    return vec3(1, f, 0);
  } else {
    return vec3(1, 1, f);
  }
  // return vec3(f);
}

// coloring the mandelbrot set using the orbit trap algorithm
vec3 mandelbrotOrbitTrap(vec2 uv, vec2 point) {
  int iter = 0;
  const int max_iter = 1000;
  float dist = 10000.0;
  // real and imaginery component of a complex number
  vec2 z = vec2(0.);

  for (int i = 0; i < max_iter; i++) {

    z = vec2(z.x * z.x - z.y * z.y, 2.0 * z.x * z.y) + uv;

    dist = min(dist, length(z - point));
  }
  // color based on the angle of the complex number;
  float f1 = dist;
  // float angle = acos(z.y / z.x);
  // float f2 = angle / 3.1415926;
  // // coloring
  // if (f1 < 0.33) {
  //   return vec3(f2, 0., f1);
  // } else if (f1 >= 0.33 && f1 <= 0.67) {
  //   return vec3(1. - f1, 1. - f2, f1 * f2);
  // } else {
  //   return vec3(1.0, f1, 1.);
  // }
  // return vec3(1 - f1, 1 - f2, f1 * f2);
  // return vec3(f1, 2 * f1, 2.9 * f1);
  return vec3(f1);
}

void main() {
  vec2 uv = 2 * (gl_FragCoord.xy - 0.5 * u_resolution.xy) / u_resolution.y;
  // uv /= pow(2.7182818, u_time / 200);
  uv += vec2(-0.748348181, 0.0);
  // Camera camera = Camera(vec3(0, 2, 1),
  // normalize(vec3(uv.x, uv.y, -1)));

  // float d = rayMarch(camera.ro, camera.rd); //
  // nearest distance to the scene

  // vec3 p = camera.ro + camera.rd * d;

  // color = lightingCalculation(p);
  // color = mandelbrotOrbitTrap(uv, vec2(0., 0.));
  color = mandelbrotEscapeTime(uv);
  // gl_FragColor = vec4(color, 1.0);
}