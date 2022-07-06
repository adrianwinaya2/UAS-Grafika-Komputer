#version 330

out vec4 outputColor;

in vec4 FragPos;
in vec3 Normal;

struct DirLight{
	vec3 direction; 

	vec3 ambient; 
	vec3 diffuse; 
	vec3 specular;
};

uniform DirLight dirLight;

struct PointLight{
	vec3 position; 

	float constant; 
	float linear; 
	float quadratic; 

	vec3 ambient; 
	vec3 diffuse; 
	vec3 specular;
};
//uniform PointLight pointLight; 

//ini utk multiple light (multiple light = banyak pointlight)
#define NR_POINT_LIGHTS 4
uniform PointLight pointLight[NR_POINT_LIGHTS]; 

struct SpotLight{
	vec3 position; 
	vec3 direction; 
	float cutOff; 
	float outerCutOff; 

	vec3 ambient; 
	vec3 diffuse; 
	vec3 specular; 

	float constant; 
	float linear; 
	float quadratic; 
}; 
uniform SpotLight spotLight;

uniform vec4 objectColor;

uniform vec3 viewPos;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir); 
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir); 
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 objCol; 

void main(){
	objCol = vec3(objectColor.x, objectColor.y, objectColor.z); 
	vec3 norm = normalize(Normal); 
	vec3 viewDir = normalize(viewPos - vec3(FragPos)); 

	vec3 result = CalcDirLight(dirLight, norm, viewDir); 

	for(int i = 0; i < NR_POINT_LIGHTS; i++)
		result += CalcPointLight(pointLight[i], norm, vec3(FragPos), viewDir); 
	result += CalcSpotLight(spotLight, norm, vec3(FragPos), viewDir); 
	outputColor = vec4(result, objectColor.w);
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
	vec3 lightDir = normalize(-light.direction);

	//diffuse shading
	float diff = max(dot(normal, lightDir), 0.0); 

	//specular shading
	vec3 reflectDir = reflect(-lightDir, normal); 
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 256); 

	//combine results
	vec3 ambient = light.ambient * objCol; 
	vec3 diffuse = light.diffuse * diff * objCol; 
	vec3 specular = light.specular * spec * objCol; 
	return (ambient + diffuse + specular); 

}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightDir = normalize(light.position - fragPos); 
	//diffuse shading
	float diff = max(dot(normal, lightDir), 0.0); 

	//specular shading
	vec3 reflectDir = reflect(-lightDir, normal); 
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 256); 

	//attenuation
	float distance = length(light.position - fragPos); 
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance)); 

	//combine results
	vec3 ambient = light.ambient * objCol; 
	vec3 diffuse = light.diffuse * diff * objCol; 
	vec3 specular = light.specular * spec * objCol; 
	ambient *= attenuation; 
	diffuse *= attenuation; 
	specular *= attenuation; 
	return (ambient + diffuse + specular); 
}

vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	//diffuse shading
	vec3 lightDir = normalize(light.position - fragPos); 
	float diff = max(dot(normal, lightDir), 0.0); 

	//specular shading
	vec3 reflectDir = reflect(-lightDir, normal); 
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 256); 

	//attenuation
	float distance = length(light.position - fragPos); 
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance)); 

	//spotlight intensity
	float theta = dot(lightDir, normalize(-light.direction)); 
	float epsilon = light.cutOff - light.outerCutOff; 
	float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0); 

	//combine results
	vec3 ambient = light.ambient * objCol; 
	vec3 diffuse = light.diffuse * diff * objCol; 
	vec3 specular = light.specular * spec * objCol; 
	ambient *= attenuation; 
	diffuse *= attenuation * intensity; 
	specular *= attenuation * intensity; 
	return (ambient + diffuse + specular); 
}