const express = require('express');
const cors = require('cors');  

const app = express();
const PORT = process.env.PORT || 5000;  

app.use(cors()); 
app.use(express.json()); 


app.get('/', (req, res) => {
    res.send('Servidor funcional');
});

app.listen(PORT, '0.0.0.0', () => {
    console.log(`Servidor a rodar na porta ${PORT}`);
});
