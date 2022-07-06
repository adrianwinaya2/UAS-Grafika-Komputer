#version 330

out vec4 outputColor;

//in vec4  vertexColor; //nerima dari shader.vert

//uniform vec4 ourColor;
uniform vec3 objColor;

void main(){
//	outputColor = vec4(1.0, 0.0, 0.0, 1.0);
    outputColor = vec4(1.0f, 0.0f, 0.0f, 1.0);
//	outputColor = vertexColor;
//	outputColor = ourColor;
}