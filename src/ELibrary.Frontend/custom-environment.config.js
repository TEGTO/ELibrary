const fs = require('fs');
const path = require('path');

const userApiUrl = process.env.USER_API || 'https://localhost:7130';
const libraryApiUrl = process.env.LIBRARY_API || 'https://localhost:7131';

const replaceInFile = (filePath, param, value) => {
    const fileContent = fs.readFileSync(filePath, 'utf8');
    const updatedContent = fileContent.replace(new RegExp(`${param}: .*`, 'g'), `${param}: '${value}',`);
    fs.writeFileSync(filePath, updatedContent, 'utf8');
    console.log(`Replaced ${param} in ${filePath}`);
};

const environmentFiles = [
    path.join(__dirname, 'src/environment/environment.ts'),
    path.join(__dirname, 'src/environment/environment.prod.ts')
];

environmentFiles.forEach(filePath => {
    replaceInFile(filePath, 'userApi', userApiUrl);
    replaceInFile(filePath, 'libraryApi', libraryApiUrl);
});
