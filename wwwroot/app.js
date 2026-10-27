const API_URL = "/api/relogios";

const form = document.getElementById("form-relogio");
const campoId = document.getElementById("id");
const campoMarca = document.getElementById("marca");
const campoModelo = document.getElementById("modelo");
const campoTipo = document.getElementById("tipo");
const campoPreco = document.getElementById("preco");
const tituloForm = document.getElementById("titulo-form");
const btnSalvar = document.getElementById("btn-salvar");
const btnCancelar = document.getElementById("btn-cancelar");
const btnRecarregar = document.getElementById("btn-recarregar");
const listaRelogios = document.getElementById("lista-relogios");
const mensagem = document.getElementById("mensagem");

const formatarPreco = (valor) =>
  valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

function mostrarMensagem(texto, tipo) {
  mensagem.textContent = texto;
  mensagem.className = tipo;
}

// GET /api/relogios
async function carregarRelogios() {
  try {
    const resposta = await fetch(API_URL);
    if (!resposta.ok) throw new Error(`Erro ${resposta.status} ao listar`);

    const relogios = await resposta.json();
    renderizarLista(relogios);
  } catch (erro) {
    mostrarMensagem(erro.message, "erro");
  }
}

function renderizarLista(relogios) {
  listaRelogios.innerHTML = "";

  if (relogios.length === 0) {
    listaRelogios.innerHTML = '<tr><td colspan="6">Nenhum relógio cadastrado.</td></tr>';
    return;
  }

  for (const relogio of relogios) {
    const linha = document.createElement("tr");
    linha.innerHTML = `
      <td>${relogio.id}</td>
      <td></td>
      <td></td>
      <td></td>
      <td>${formatarPreco(relogio.preco)}</td>
      <td class="botoes">
        <button type="button" class="secundario">Editar</button>
        <button type="button" class="perigo">Excluir</button>
      </td>`;
    // textContent evita injetar HTML vindo dos dados
    linha.children[1].textContent = relogio.marca;
    linha.children[2].textContent = relogio.modelo;
    linha.children[3].textContent = relogio.tipo;

    const [btnEditar, btnExcluir] = linha.querySelectorAll("button");
    btnEditar.addEventListener("click", () => editarRelogio(relogio.id));
    btnExcluir.addEventListener("click", () => excluirRelogio(relogio));

    listaRelogios.appendChild(linha);
  }
}

// GET /api/relogios/{id} para preencher o formulário de edição
async function editarRelogio(id) {
  try {
    const resposta = await fetch(`${API_URL}/${id}`);
    if (resposta.status === 404) throw new Error(`Relógio ${id} não encontrado (404)`);
    if (!resposta.ok) throw new Error(`Erro ${resposta.status} ao buscar`);

    const relogio = await resposta.json();
    campoId.value = relogio.id;
    campoMarca.value = relogio.marca;
    campoModelo.value = relogio.modelo;
    campoTipo.value = relogio.tipo;
    campoPreco.value = relogio.preco;

    tituloForm.textContent = `Editando relógio #${relogio.id}`;
    btnSalvar.textContent = "Salvar alterações";
    btnCancelar.hidden = false;
    mostrarMensagem("", "");
    campoMarca.focus();
  } catch (erro) {
    mostrarMensagem(erro.message, "erro");
    carregarRelogios();
  }
}

function limparFormulario() {
  form.reset();
  campoId.value = "";
  tituloForm.textContent = "Cadastrar relógio";
  btnSalvar.textContent = "Cadastrar";
  btnCancelar.hidden = true;
}

// POST /api/relogios (novo) ou PUT /api/relogios/{id} (edição)
form.addEventListener("submit", async (evento) => {
  evento.preventDefault();

  const dados = {
    marca: campoMarca.value.trim(),
    modelo: campoModelo.value.trim(),
    tipo: campoTipo.value,
    preco: Number(campoPreco.value)
  };

  const id = campoId.value;
  const editando = id !== "";

  try {
    const resposta = await fetch(editando ? `${API_URL}/${id}` : API_URL, {
      method: editando ? "PUT" : "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(dados)
    });

    if (resposta.status === 404) throw new Error(`Relógio ${id} não encontrado (404)`);
    if (!resposta.ok) throw new Error(`Erro ${resposta.status} ao salvar`);

    const salvo = await resposta.json();
    mostrarMensagem(
      editando
        ? `Relógio #${salvo.id} atualizado (${resposta.status} OK)`
        : `Relógio #${salvo.id} cadastrado (${resposta.status} Created)`,
      "sucesso"
    );
    limparFormulario();
  } catch (erro) {
    mostrarMensagem(erro.message, "erro");
  }

  carregarRelogios();
});

// DELETE /api/relogios/{id}
async function excluirRelogio(relogio) {
  if (!confirm(`Excluir o relógio ${relogio.marca} ${relogio.modelo}?`)) return;

  try {
    const resposta = await fetch(`${API_URL}/${relogio.id}`, { method: "DELETE" });
    if (resposta.status === 404) throw new Error(`Relógio ${relogio.id} não encontrado (404)`);
    if (!resposta.ok) throw new Error(`Erro ${resposta.status} ao excluir`);

    mostrarMensagem(`Relógio #${relogio.id} excluído (${resposta.status} No Content)`, "sucesso");
    if (campoId.value === String(relogio.id)) limparFormulario();
  } catch (erro) {
    mostrarMensagem(erro.message, "erro");
  }

  carregarRelogios();
}

btnCancelar.addEventListener("click", limparFormulario);
btnRecarregar.addEventListener("click", carregarRelogios);

carregarRelogios();
