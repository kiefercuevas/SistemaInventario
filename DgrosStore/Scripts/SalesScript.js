$(document).ready(function () {

    //para efectos en cliente y producto
    $("#searchClient").click(AddInputEffect);
    $("#searchProduct").click(AddInputEffect);

    //clientes y productos
    $("#clientInput").keyup(function () {
        var client = $("#clientInput").val();
        var selectClient = $("#clientSelect");
        $.ajax({
            type: "GET",
            url: "Sales/GetClients",
            data: { client: client },
            success: function (data) {

                data = JSON.parse(data);
                if (data.length == 0) {
                    selectClient.html("<option value>no hay clientes con ese nombre</option>");
                } else {
                    selectClient.html("");
                    $(data).each(function (index, item) {
                        selectClient.append(`<option value="${item.id}">${item.name + " " + item.lastName}</option>`);
                    });
                }

            }
        });

    });
    $("#productInput").keyup(function () {
        var product = $("#productInput").val();
        var selectProduct = $("#productSelect");
        $.ajax({
            type: "GET",
            url: "Sales/GetProducts",
            data: { product: product },
            success: function (data) {

                data = JSON.parse(data);
                if (data.length == 0) {
                    selectProduct.html("<option value>no hay productos con ese nombre</option>");
                } else {
                    selectProduct.html("");
                    $(data).each(function (index, item) {
                        selectProduct.append(`<option value="${item.id}">${item.name}</option>`);
                    });
                }

            }
        });

    });




    //añadir productos
    $("#addProduct").click(function () {
        var selectedProduct = $("#productSelect").val();

        if (selectedProduct > 0) {
            $.ajax({
                type: "GET",
                url: "Sales/GetProduct",
                data: { id: selectedProduct },
                success: function (data) {

                    data = JSON.parse(data);

                    if (!RepeatedProduct(data)) {

                        //append table element
                        CreateTableElement(data)

                        //price
                        UpdatePrice();

                        //productos
                        CreateProductArray();

                        //quantity
                        EventAsingToQuantityAndDiscount(data);


                        //delete button
                        CreateDeleteButtonArray(data);

                    } else {
                        alert("el producto ya esta en la lista de productos");
                    }
                }
            });
        } else {
            alert("debe elegir un producto");
        }
    });



    //funciones


    //efectos en los input de client y product
    function AddInputEffect() {

        var input = $(this).parent().siblings("input[type='text']");
        var icon = $(this).children("i");

        $(icon).toggleClass("fa fa-remove");
        $(icon).toggleClass("fa fa-search");
        $(input).toggleClass("show");
        $(input).val("");
    }

    //crear elemento de tr de la tabla
    function CreateTableElement(data) {

        $("#tableProduct").append(
            `<tr>
                        <td style="display:none"><input type="hidden" value="${data.id}" readonly></td>
                        <td><strong>${data.name}</strong></td>
                        <td><input type="number" name="quantity" min="1" max="${data.stock}" value="1"></td>
                        <td><input type="number" name="discount" min="0" max="100" value="0"></td>
                        <td><strong name="price">${data.price}</strong></td>
                        <td><strong name="subTotal">${data.price}</strong></td>
                        <td><button type="button" class="btn btn-danger btn-sm"><span class="fa fa-remove"></span></button></td>
                     </tr>`
        );
    }

    //crear Arreglo de id de Productos
    function CreateProductArray() {
        var products = Array.from($("#tableProduct")
            .children("tr")
            .children("td")
            .children("input[type='hidden']"));

        return products;
    }

    //ver si se repite el producto
    function RepeatedProduct(data) {

        var products = CreateProductArray();
        var productRepeated = false;

        $(products).each(function (index, item) {
            if ($(item).val() == data.id) {
                productRepeated = true;
            }
        });
        return productRepeated;
    }


    //actualizar precio
    function UpdatePrice() {

        var total = $("#total");
        var price = 0;

        var ProductPrice = Array
            .from($("#tableProduct")
                .children("tr")
                .children("td")
                .children("strong[name='subTotal']"));

        $(ProductPrice).each(function (i, priceItem) {
            price += parseInt($(priceItem).html());
        });


        total.html(price);

    }



    //asignar eventos al input cantidades y descuentos
    function EventAsingToQuantityAndDiscount(data) {

        var Productquantity = $("#tableProduct").children("tr").children("td").children("input[name='quantity']");
        var Discountquantity = $("#tableProduct").children("tr").children("td").children("input[name='discount']");
        //var subTotal = $(this).parent().next().next().next().children("strong[name='subTotal']");


        $(Productquantity).last().on("keyup click", data, function () {
            var element = this;
            PriceCal(element, data);
            //price
            UpdatePrice();
        });

        $(Discountquantity).last().on("keyup click", data, function () {
            var element = this;
            PriceCal(element, data);
            //price
            UpdatePrice();
        });


    }


    //calcular precio
    function PriceCal(element, data) {

        var subTotal = $(element).parent().parent().children("td").children("strong[name='subTotal']");
        var discountInput = $(element).parent().parent().children("td").children("input[name='discount']").val();
        var quantityInput = $(element).parent().parent().children("td").children("input[name='quantity']").val();
        var percent = parseFloat(parseInt(discountInput) / 100);

        var discount = parseFloat(data.price * percent);

        var totalProductPriceWithDiscount = (data.price * quantityInput) - (discount * quantityInput);
        $(subTotal).html(totalProductPriceWithDiscount);
    }

    //borrar productos
    function CreateDeleteButtonArray(data) {

        var deleteButton = $("#tableProduct")
            .children("tr")
            .children("td")
            .children("button");

        $(deleteButton).last().on("click", data, function (i, item) {
            if (confirm("Desea borrar este producto?")) {
                $(this).parent().parent().remove();
                UpdatePrice();
            }
        });
    }

});